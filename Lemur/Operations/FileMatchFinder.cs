using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Lemur.Tasks;

using static Lemur.Debug.DebugUtils;
using Lemur.Utils;

namespace TorboFile {

	/// <summary>
	/// Removes duplicate copies of a file within a given directory.
	/// </summary>
	public class FileMatchFinder : ProgressOperation {

		[Flags]
		public enum MatchOptions {

			/// <summary>
			/// If set, file contents are searched to compare files.
			/// Set by default.
			/// </summary>
			MatchContents = 1,

			/// <summary>
			/// If set, directories are recursively searched for
			/// matching files.
			/// </summary>
			Recursive = 2,

			/// <summary>
			/// If set, duplicate matches are deleted after a search.
			/// Not set by default.
			/// </summary>
			AutoDelete = 4,

			/// <summary>
			/// If set, deleted files are moved to trash instead
			/// of being deleted directly.
			/// Not set by default.
			/// </summary>
			MoveToTrash = 8

		}

		/// <summary>
		/// Amount of file-size that counts towards one point of operation progress.
		/// This is used because counting raw file sizes could too easily overflow
		/// the progress variable.
		/// </summary>
		private const long SIZE_PER_PROGRESS = 5*1024;

		private int maxChunkSize = 4096 * 1024;     /// approx. 4MB

		/// <summary>
		/// Maximum size of data chunks read from files
		/// being compared.
		/// </summary>
		public int MaxChunkSize {
			get {
				return this.maxChunkSize;
			}
			set {
				this.maxChunkSize = value;
			}
		}

		/// <summary>
		/// Collection of groups of file matches.
		/// Each group contains files that all match the search criteria.
		/// </summary>
		private readonly MatchCollection matches = new MatchCollection();
		public MatchCollection Matches {
			get { return this.matches; }
		}

		/// <summary>
		/// Lock for accessing matches.
		/// </summary>
		private object collectionLock;
		public void SetLock( object matchesLock ) {
			this.collectionLock = matchesLock;
		}

		public bool RecursiveSearch {
			get {
				return this.options.HasFlag( MatchOptions.Recursive );
			}
			set {
				this.SetOption( MatchOptions.Recursive, value );
			}
		}

		/// <summary>
		/// Whether duplicate files should automatically be deleted.
		/// true by default.
		/// </summary>
		public bool AutoDeleteCopies {
			get {
				return this.options.HasFlag( MatchOptions.AutoDelete );
			}
			set {
				this.SetOption( MatchOptions.AutoDelete, value );
			}
		}

		private MatchOptions options = MatchOptions.AutoDelete | MatchOptions.MatchContents | MatchOptions.MoveToTrash;
		public MatchOptions Options {
			get {
				return this.options;
			}
			set {
				this.options = value;
			}
		}

		/// <summary>
		/// Limits matches to files with the given extensions.
		/// </summary>
		private string[] fileTypes;
		/// <summary>
		/// Limits matches to files with the given extensions.
		/// </summary>
		public string[] FileTypes {
			get {
				return this.fileTypes;
			}
			set {
				this.fileTypes = value;
			}
		}

		/// <summary>
		/// Excludes files with the given file extensions.
		/// </summary>
		private string[] excludeTypes;
		/// <summary>
		/// Excludes files with the given file extensions.
		/// </summary>
		public string[] ExcludeTypes {
			get {
				return this.excludeTypes;
			}
			set {
				this.excludeTypes = value;
			}
		}

		/// <summary>
		/// Minimum size of files to check.
		/// </summary>
		private long minFileSize = 0;

		/// <summary>
		/// Maximum sizes of file to check.
		/// </summary>
		private long maxFileSize = long.MaxValue;

		/// <summary>
		/// Sets the size range when searching for file matches.
		/// Files outside the size range are ignored.
		/// If the maximum size is set to a negative value,
		/// it is ignored.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public void SetSizeRange( long min, long max=-1 ) {

			if ( max < 0 ) {
				max = long.MaxValue;
			} else if ( max < min ) {
				long t = max;
				max = min;
				min = t;
			}

			if ( min <= 0 ) {
				min = 0;
			}

			this.minFileSize = min;
			this.maxFileSize = max;

		}

		/// <summary>
		/// Starting directory of the clean.
		/// </summary>
		private string _baseSearchDir;

		/// <summary>
		/// Whether the contents of files should be compared when reporting file matches.
		/// true by default.
		/// </summary>
		public bool MatchContents {
			get {
				return this.options.HasFlag( MatchOptions.MatchContents );
			}
			set {
				this.SetOption( MatchOptions.MatchContents, value );
			}
		}

		private void SetOption( MatchOptions opt, bool enabled ) {
			if ( enabled ) {
				this.options |= opt;
			} else {
				this.options &= ~( opt );
				//Log( "opts: " + this.options );
			}
		}

		/// <summary>
		/// Creates a new FileMatch operation starting from the given directory,
		/// with the given Matching options.
		/// </summary>
		/// <param name="dirPath"></param>
		/// <param name="options"></param>
		public FileMatchFinder( string dirPath, MatchOptions options ) {

			this._baseSearchDir = dirPath;
			this.options = options;

		}

		public FileMatchFinder( string directoryPath, bool recursive = false ) {

			this._baseSearchDir = directoryPath;
			this.RecursiveSearch = recursive;

		}

		/// <summary>
		/// Runs the FileMatchFinder operation.
		/// If a lock is provided, it can be used to access Results
		/// elements while the Finder is running.
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="lockObj"></param>
		public void Run( object lockObj ) {

			this.collectionLock = lockObj;

			this.Run();

		}

		/// <summary>
		/// Runs a file-match search.
		/// Returns the total size of all duplicate files.
		/// </summary>
		override public void Run() {

			try {

				this.FindAllMatches();

			} catch( Exception ) {

			} finally {

				/// Mark operation as complete.
				this.OperationComplete();

			}

		} //

		private void FindAllMatches() {

			ICollection<FileMatchGroup> matches = this.matches;

			List<FileData> fileSizes = this.GetFileSizes( this._baseSearchDir, this.RecursiveSearch );
			int totalFiles = fileSizes.Count;

			for ( int fileIndex = 0; fileIndex < totalFiles; fileIndex++ ) {

				FileData curFile = fileSizes[fileIndex];
				if ( string.IsNullOrEmpty(curFile.path) ) {
					// file was already assigned to a group. messy marker...
					continue;
				}
				
				FileMatchGroup match = this.FindMatches( fileSizes, curFile, fileIndex );
				if ( match != null ) {
					this.AddMatchGroup( match );
				}

			} // for-loop

		} //

		/// <summary>
		/// Private method runs the logic of finding mathces.
		/// </summary>
		/// <param name="allFiles"></param>
		/// <param name="curFile"></param>
		/// <param name="fileIndex"></param>
		/// <returns></returns>
		private FileMatchGroup FindMatches( List<FileData> allFiles, FileData curFile, int fileIndex ) {

			FileMatchGroup matchGroup = null;
			int totalFiles = allFiles.Count;
			long fileSize = curFile.size;

			for ( int nextIndex = fileIndex + 1; nextIndex < totalFiles; nextIndex++ ) {

				FileData nextFile = allFiles[nextIndex];
				this.AdvanceFileProgress( nextFile.size );

				if ( string.IsNullOrEmpty( nextFile.path ) ) {
					// file was already used in a match.
					continue;
				} else if ( nextFile.size > fileSize ) {
					// different file size. no more matches to find.
					break;
				}

				/// POSSIBLE FILE MATCH.
				if ( this.CompareFiles( curFile.path, nextFile.path, fileSize ) ) {

					if ( matchGroup == null ) {
						matchGroup = new FileMatchGroup( fileSize, curFile.path );
					}

					matchGroup.Add( nextFile.path );
					/// clear the entry so it isn't used again.
					allFiles[nextIndex] = new FileData();

				}
				if ( this.CancelRequested() ) {
					break;
				}

			} // for-loop

			return matchGroup;

		}

		/// <summary>
		/// Adds a match group to the Matches collection.
		/// If a collection lock exists, the matches collection is
		/// locked before access.
		/// </summary>
		/// <param name="group"></param>
		private void AddMatchGroup( FileMatchGroup group ) {

			if( this.collectionLock != null ) {

				this.Dispatch( ()=> {

					lock( this.collectionLock ) {

						this.matches.Add( group );

					}
				}

				);

			} else {
				this.matches.Add( group );
			}

		}

		/// <summary>
		/// Compares the contents of two files to determine if they are identical.
		/// The files being compared must have the same file sizes.
		/// </summary>
		/// <returns>true if the two files have the same contents, false otherwise.</returns>
		private bool CompareFiles( string f1, string f2, long fileSize ) {

			int chunkSize = this.maxChunkSize;
			if ( fileSize < chunkSize ) {
				chunkSize = (int)fileSize;
			}

			byte[] chunk1 = new byte[chunkSize];
			byte[] chunk2 = new byte[chunkSize];

			using ( FileStream file1 = File.OpenRead( f1 ) ) {

				using ( FileStream file2 = File.OpenRead( f2 ) ) {

					while( true ) {

						int bytesRead1 = file1.Read( chunk1, 0, chunkSize );
						int bytesRead2 = file2.Read( chunk2, 0, chunkSize );

						// bytes read should be equal since the file sizes are equal, but maybe one file
						// was changed, or there was an error.
						if ( bytesRead1 != bytesRead2 ) {
							return false;
						} else if ( bytesRead1 == 0 ) {
							// end of file.
							break;
						}

						if ( this.CancelRequested() ) {
							return false;
						}

						if ( !this.CompareFileChunks( chunk1, chunk2, bytesRead1 ) ) {
							return false;
						}

					} // while


				} // using

			} // using

			return true;

		}

		/// <summary>
		/// Returns a list of fileNames/sizes sorted by fileSize.
		/// </summary>
		/// <param name="baseDir"></param>
		/// <param name="recursive"></param>
		/// <returns></returns>
		private List<FileData> GetFileSizes( string baseDir, bool recursive ) {

			string[] filePaths;
			if ( recursive ) {
				filePaths = FileUtils.GetFiles( baseDir, "*" );
			} else {
				filePaths = Directory.GetFiles( baseDir );
			}

			this.SetMaxProgress( filePaths.Length );

			/// sort files by file size to compare files of the same size.
			List<FileData> files = new List<FileData>( filePaths.Length );
			long size;

			for ( int i = filePaths.Length - 1; i >= 0; i-- ) {
				
				try {

					FileInfo info = new FileInfo( filePaths[i] );
					size = info.Length;

					if( !this.ExcludeFile( info ) ) {

						this.AddFileProgress( size );

						files.Add( new FileData( info.FullName, size ) );
					}

				} catch( Exception ) {
				}

				this.AdvanceProgress();

				if ( this.CancelRequested() ) {
					break;
				}

			}

			if ( this.CancelRequested() ) {
				files.Clear();
				return files;
			}
			//Log( "Find Max Progress: " + files.Count );

			files.Sort( ( x, y ) => {

				if ( x.size > y.size ) {
					return 1;
				} else if ( y.size > x.size ) {
					return -1;
				}
				return 0;

			} );

			return files;

		}

		/// <summary>
		/// A a file size contribution to the maximum progress.
		/// </summary>
		/// <param name="fileSize"></param>
		private void AddFileProgress( long fileSize ) {

			// adding full file size might overflow a long.
			this.AdvanceMaxProgress( fileSize / SIZE_PER_PROGRESS );

		}

		/// <summary>
		/// Adds a file's size to the current amount of progress
		/// which has been made.
		/// </summary>
		/// <param name="fileSize"></param>
		private void AdvanceFileProgress( long fileSize ) {
			this.AdvanceProgress( fileSize / SIZE_PER_PROGRESS );
		} //

		/// <summary>
		/// Checks if a file should be excluded from matches
		/// based on file extension or file size.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private bool ExcludeFile( FileInfo info ) {

			if( this.excludeTypes != null || this.fileTypes != null ) {

				string ext = Path.GetExtension( info.Name ).ToLower();

				if( this.fileTypes != null && !this.fileTypes.Contains( ext ) ) {
					Console.WriteLine( "Excluding: " + ext );
					return true;
				}
				if( this.excludeTypes != null && this.excludeTypes.Contains( ext ) ) {
					return true;
				}

			}

			long len = info.Length;
			if( len < this.minFileSize || len > this.maxFileSize ) {
				return true;
			}

			return false;

		} //

		/// <summary>
		/// Checks if two file chunks are identical.
		/// </summary>
		/// <param name="chunk1"></param>
		/// <param name="chunk2"></param>
		/// <param name="maxIndex"></param>
		/// <returns>true if the chunks from files are identical, false otherwise.</returns>
		private bool CompareFileChunks( byte[] chunk1, byte[] chunk2, int maxIndex ) {

			for ( int i = 0; i < maxIndex; i++ ) {
				if ( chunk1[i] != chunk2[i] ) {
					return false;
				}
			}
			return true;

		}

	} // class

} // namespace