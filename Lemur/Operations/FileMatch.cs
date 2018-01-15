using Lemur.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur {

	/// <summary>
	/// Options for how to delete and order files in a MatchGroup.
	/// </summary>
	[Flags]
	public enum MatchOrdering {

		None = 0,

		/// <summary>
		/// Delete duplicate files in alphabetical order.
		/// </summary>
		Lexigraphic = 1,

		/// <summary>
		/// Delete duplicate files in reverse alphabetical order.
		/// Files whose names come last in the ordering are deleted first.
		/// </summary>
		ReverseLexigraphic = 2,

		/// <summary>
		/// TODO: Delete files with the shortest names first.
		/// </summary>
		ShortestName = 4,

		/// <summary>
		/// TODO: Delete files with longer names first.
		/// </summary>
		LongestName = 8,

		/// <summary>
		/// TODO: Numeric digits are treated as coming lexigraphically
		/// _after_ letters. By default digits come first.
		/// </summary>
		NumbersAfterLetters = 16

	} // MatchOrdering

	/// <summary>
	/// A collection of FileMatchGroups.
	/// Each FileMatchGroup is itself a collection of files that match within the group.
	/// </summary>
	public class MatchCollection : ObservableCollection<FileMatchGroup> {

		/// <summary>
		/// Returns a list of all files from every FileMatchGroup in the collection.
		/// Note that the files returned are not all guaranteed to match as they
		/// will come from different MatchGroups.
		/// </summary>
		/// <returns></returns>
		/*public List<FileData> GetFiles() {

			List<FileData> files = new List<FileData>();
			long fileSize;

			foreach ( FileMatchGroup g in this ) {

				fileSize = g.FileSize;

				foreach ( string path in g ) {
					
					files.Add( new FileData( path, fileSize ) );

				}

			}

			return files;

		}*/

		/// <summary>
		/// Total size of all duplicate files.
		/// ( All but one file from every group of matching files. )
		/// </summary>
		public long DuplicatesSize {

			get {
				long totalSize = 0;
				for ( int i = this.Count - 1; i >= 0; i-- ) {
					totalSize += this[i].DuplicatesSize;
				}
				return totalSize;
			}

		}

		/// <summary>
		/// Total size of all files in all file groups.
		/// </summary>
		public long TotalSize {

			get {
				long totalSize = 0;
				for ( int i = this.Count - 1; i >= 0; i-- ) {
					totalSize += this[i].TotalSize;
				}
				return totalSize;
			}

		}


		/// <summary>
		/// Sorts the matches in every matching group according to a given order.
		/// </summary>
		/// <param name="order"></param>
		public void SortMatches( MatchOrdering order ) {

			if ( order == MatchOrdering.None ) {
				return;
			}
			foreach ( FileMatchGroup match in this ) {
				match.SortMatches( order );
			}

		} //

		/// <summary>
		/// Deletes all but one file from every group of matches.
		/// Returns the total size of deleted items.
		/// </summary>
		/// <param name="matches"></param>
		public long DeleteMatches( bool moveToTrash=false, MatchOrdering deleteOrder = MatchOrdering.None ) {

			long totalSize = 0;

			this.SortMatches( deleteOrder );

			foreach ( FileMatchGroup match in this ) {

				totalSize += match.DuplicatesSize;
				string[] files = match.RemoveMatches();

				int len = files.Length;

				/// don't delete the first file from each group.
				for ( int i = 0; i < len; i++ ) {

					if ( moveToTrash ) {
						RecycleBinDeleter.Delete( files[i] );
					} else {
						File.Delete( files[i] );
					}

				} // for-loop.

			} // for-loop.

			return totalSize;

		} //

	} // class

	/// <summary>
	/// Group of files that all match the grouping criteria.
	/// </summary>
	public class FileMatchGroup : ICollection<string> {

		private long fileSize;
		/// <summary>
		/// Size of each file in the group.
		/// All files in a group should have the same file size.
		/// </summary>
		public long FileSize {
			get {
				return this.fileSize;
			}
		}

		private List<string> filePaths;

		/// <summary>
		/// List of files in the group.
		/// </summary>
		public string[] FilePaths {
			get {
				return filePaths.ToArray();
			}
		}

		/// <summary>
		/// Returns the total size of all files in the group.
		/// </summary>
		public long TotalSize {
			get {
				return this.fileSize * this.filePaths.Count;
			}
		}


		/// <summary>
		/// Total size of all duplicate files. ( All files in the group except one. )
		/// </summary>
		public long DuplicatesSize {
			get {
				return this.fileSize * ( this.filePaths.Count - 1 );
			}
		}

		public int Count => this.filePaths.Count;

		public bool IsReadOnly => false;

		public void SortMatches( MatchOrdering order ) {
			
			if ( order == MatchOrdering.ReverseLexigraphic ) {

				this.filePaths.Sort(
					/// later lexigraphic names (e.g. 'z') come first.
					( a, b ) => { return string.Compare( b, a, StringComparison.CurrentCulture ); }
				);

			} else {

				this.filePaths.Sort(
					( a, b ) => { return string.Compare( a, b, StringComparison.CurrentCulture ); }
				);

			}

		}

		/// <summary>
		/// Removes all but the a single file from the match group and returns the
		/// paths of the removed files.
		/// If there is one file or less in the group, an empty
		/// array is returned.
		/// </summary>
		/// <returns></returns>
		public string[] RemoveMatches() {

			int removeCount = this.filePaths.Count - 1;
			if ( removeCount <= 0 ) {
				return new string[0];
			}

			string[] removed = filePaths.GetRange( 0, removeCount ).ToArray();
			filePaths.RemoveRange( 0, removeCount );

			return removed;

		}

		public FileMatchGroup( long size, string fileName ) : this( size ) {

			this.filePaths.Add( fileName );

		}

		public FileMatchGroup( long size ) {

			this.fileSize = size;
			this.filePaths = new List<string>();

		}

		public void Add( string filePath ) {
			this.filePaths.Add( filePath );
		}

		public void Clear() {
			this.filePaths.Clear();
		}

		public bool Contains( string item ) {
			return this.filePaths.Contains( item );
		}

		public void CopyTo( string[] array, int arrayIndex ) {
			this.filePaths.CopyTo( array, arrayIndex );
		}

		public bool Remove( string item ) {
			return this.filePaths.Remove( item );
		}

		public IEnumerator<string> GetEnumerator() {
			return this.filePaths.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.filePaths.GetEnumerator();
		}

	} // class

	/// <summary>
	/// Basic name/size information about a duplicate file.
	/// </summary>
	public struct FileDuplicateInfo : IEquatable<FileDuplicateInfo> {

		public string path;
		public string Path {
			get {
				return this.path;
			}
		}

		public long size;
		public long Size {
			get {
				return this.size;
			}
		}


		private int groupID;
		public int Group {
			get { return this.groupID; }
			set { this.groupID = value; }
		}

		public FileDuplicateInfo( string filePath, long fileSize = 0, int group=0 ) {

			this.path = filePath;
			this.size = fileSize;
			this.groupID = group;

		}

		public bool Equals( FileDuplicateInfo other ) {
			return other.path == this.path;
		}

		public override bool Equals( object obj ) {

			if ( obj is FileDuplicateInfo ) {
				FileDuplicateInfo other = (FileDuplicateInfo)obj;
				return other.path == this.path;
			}
			return false;
		}

		public override int GetHashCode() {
			return this.path.GetHashCode();
		}

		public static bool operator ==( FileDuplicateInfo x, FileDuplicateInfo y ) {
			return x.path == y.path;
		}

		public static bool operator !=( FileDuplicateInfo x, FileDuplicateInfo y ) {
			return x.path != y.path;
		}

	} // FileInfo

} // namespace