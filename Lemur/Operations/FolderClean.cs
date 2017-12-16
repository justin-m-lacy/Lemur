using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lemur.Tasks;
using Lemur.Types;
using Lemur.Operations.FileMatching;

namespace TorboFile {

	/// <summary>
	/// Cleans the contents of a Folder according to a set of criteria.
	/// </summary>
	public class FolderClean : ProgressOperation {

		private string folderPath;

		/// <summary>
		/// List of empty directories that could not be deleted.
		/// </summary>
		private List<string> errorList;
		public string[] ErrorList {
			get { return this.errorList.ToArray(); }
		}
	
		/// <summary>
		/// Settinsg used for the operation.
		/// </summary>
		private FileMatchSettings settings;

		/// <summary>
		/// Reports when a folder-delete attempt completes.
		/// </summary>
		private Action<string, bool> reportDelete;

		/// <summary>
		/// List of empty directories that were successfully deleted.
		/// </summary>
		private List<string> deletedList;
		public string[] DeletedList {
			get { return this.deletedList.ToArray(); }
		}

		/// <summary>
		/// Initializes a FolderClean operation, including removing files in the given size range.
		/// </summary>
		/// <param name="directory"></param>
		/// <param name="range"></param>
		/// <param name="recursive"></param>
		public FolderClean( string directory, DataRange range, bool recursive = false )
			: this ( directory, new FileMatchSettings{ UseSizeRange = true, SizeRange = range, Recursive = recursive } ){
		}

		public FolderClean( string directory, FileMatchSettings settings ) {

			this.folderPath = directory;
			this.settings = settings;

			this.errorList = new List<string>();
			this.deletedList = new List<string>();

		}

		public FolderClean( string directory, bool recursive=false ) : this( directory, new FileMatchSettings { Recursive = recursive } ) {
		} // FolderClean()

		/// <summary>
		/// Run operation, reporting to calling thread when a folder delete is attempted.
		/// TODO: move the actual delete operation into a sub-operation.
		/// </summary>
		/// <param name="onDelete"></param>
		public void Run( Action<string, bool> onDelete ) {

			this.reportDelete = onDelete;
			this.Run();

		}

		/// <summary>
		/// Deletes empty subdirectories of the directory
		/// specified in the constructor.
		/// If the recursive option is set, empty descendent
		/// directories are also deleted.
		/// </summary>
		override public void Run() {

			if( !Directory.Exists( this.folderPath ) ) {
				this.OperationComplete();
				return;
			}

			try {

				this.TryDeleteFiles( this.folderPath );

				// the initial folder is never deleted.
				string[] dirs = Directory.GetDirectories( this.folderPath );

				foreach ( string subdir in dirs ) {

					// delete any empty subdirectories.
					this.DeleteIfEmpty( subdir, this.settings.Recursive );

				}
	
			} catch ( Exception ) {
			}


		} //

		/// <summary>
		/// Attempts to delete the file at the given filePath, if
		/// its size is in the specified sizerange.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private bool TryDeleteFile( string filePath ) {

			FileInfo info = new FileInfo( filePath );
			long size = info.Length;

			if( (size > 0 || !this.settings.DeleteEmptyFiles ) &&
				!this.settings.SizeRange.Contains( size ) ) {
				return false;
			}

			this.Delete( filePath, true );

			return true;

		}

		/// <summary>
		/// Deletes all files in the given size range from the given directory,
		/// and returns the number of files remaining.
		/// </summary>
		/// <param name="parentDir"></param>
		/// <returns></returns>
		private int TryDeleteFiles( string parentDir ) {

			try {

				string[] files = Directory.GetFiles( parentDir );

				// no size range or empty files will be deleted, so no chance to delete anything.
				if( !this.settings.UseSizeRange && !this.settings.DeleteEmptyFiles ) { return files.Length; }

				int fileCount = files.Length;
				for( int i = fileCount - 1; i >= 0; i-- ) {

					string file = files[i];

					try {

						if( this.TryDeleteFile( file ) ) {
							fileCount--;
						}

					} catch( Exception ) {

					}

				} // for-loop.

				return fileCount;

			} catch( Exception ) {

				// If there's an exception listing the files, need to return 1 to ensure the directory
				// isn't deleted.
				return 1;

			} //

		}

		/*private void CleanFolder( string directory, bool recursive ) {

			int fileCount = this.TryDeleteFiles( directory );

		}*/

		/// <summary>
		/// Attempts to delete any empty folders in the given directory,
		/// and returns the number of directories remaining.
		/// </summary>
		/// <param name="parentDir"></param>
		/// <returns></returns>
		/*private int TryDeleteDirs( string parentDir, bool recursive ) {

			try {

				string[] dirs = Directory.GetDirectories( parentDir );
				int dirCount = dirs.Length;


				if( !recursive ) {
					return dirCount;
				}

				return dirCount;

			} catch( Exception ) {
				// prevent this directory from being deleted in the event of an error.
				return 1;
			}

		}*/

		/// <summary>
		/// Deletes the specified directory if it contains no folders, or files that
		/// can't be deleted,
		/// or if the recursive option is true, and no descendent directories contain
		/// files or folders that can't be deleted.
		/// </summary>
		/// <param name="directory"></param>
		/// <returns>true if the directory was deleted, false otherwise.</returns>
		private bool DeleteIfEmpty( string directory, bool recursive ) {

			string[] dirs = Directory.GetDirectories( directory );
			int dirCount = dirs.Length;

			if ( recursive ) {

				foreach ( string subdir in dirs ) {

					if ( DeleteIfEmpty( subdir, recursive ) ) {
						dirCount--;
					}

				}

			}

			int fileCount = this.TryDeleteFiles( directory );

			if ( fileCount == 0 && dirCount == 0 ) {
				return this.Delete( directory );
			}

			return false;

		}

		private void DispatchDelete( string path, bool success ) {

			Action<string, bool> report = this.reportDelete;
			if( report != null ) {

				this.Dispatch( () => {
					report( path, success );
				} );

			}

		} //

		/// <summary>
		/// Performs the delete operation.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="isFile"></param>
		private bool Delete( string path, bool isFile=false ) {

			bool success = false;

			try {

				if( this.settings.MoveToTrash ) {

					success = RecycleBinDeleter.Delete( path );

				} else if( isFile ) {


					File.Delete( path );
					success = true;

				} else {


					Directory.Delete( path );
					success = true;

				}
				this.deletedList.Add( path );
				this.DispatchDelete( path, success );


			} catch( Exception ) {

				this.errorList.Add( path );
				this.DispatchDelete( path, false );

			}

			return success;

		} // DeleteFiles()

	} // class

} // namespace