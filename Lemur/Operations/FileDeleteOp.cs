using Lemur.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Lemur.Debug.DebugUtils;
using Lemur.Windows;

namespace Lemur {

	/// <summary>
	/// Deletes a group of files while reporting progress.
	/// NOTE: Updated to allow for deleting directories as well as files.
	/// </summary>
	public class FileDeleteOp : ProgressOperation {

		/// <summary>
		/// List of files to delete.
		/// </summary>
		private List<string> toDelete;
		private List<Exception> errors;

		/// <summary>
		/// List of any files that could not be deleted in the operation.
		/// </summary>
		private List<string> failedPaths;

		/// <summary>
		/// Whether to move deleted files to trash, instead of
		/// deleting them automatically.
		/// </summary>
		private bool _moveToTrash;
		public bool MoveToTrash {
			get {
				return this._moveToTrash;
			}
			set { this._moveToTrash = value; }
		}

		public string[] FailedToDelete {
			get {
				return this.failedPaths.ToArray();
			}
		}

		public Exception[] Errors {
			get {
				return this.errors.ToArray();
			}
		}

		/// <summary>
		/// Initialize with a list of files to delete.
		/// </summary>
		/// <param name="paths"></param>
		public FileDeleteOp( IEnumerable<string> paths, bool moveToTrash=false ) {

			this.toDelete = new List<string>( paths );

			this.SetMaxProgress( toDelete.Count );

			this.MoveToTrash = moveToTrash;
		}

		public override void Run() {

			this.errors = new List<Exception>();
			this.failedPaths = new List<string>();

			this.DeleteFiles();

			this.OperationComplete();

		}

		/// <summary>
		/// Method performs the actual delete operation.
		/// </summary>
		private void DeleteFiles() {

			bool moveToTrash = this.MoveToTrash;
			int len = this.toDelete.Count;
			for ( int i = 0; i < len; i++ ) {

				string path = this.toDelete[i];

				try {

					if( moveToTrash ) {

						if( !RecycleBinDeleter.Delete( path ) ) {
							/// recycleBindDelete could not delete.
							this.failedPaths.Add( path );
						}

					} else if( File.Exists( path ) ) {

						File.Delete( path );

					} else if( Directory.Exists( path ) ) {

						Directory.Delete( path );

					}

				} catch ( Exception e ) {

					Log( e.ToString() );
					this.errors.Add( e );
					this.failedPaths.Add( path );

				}

				this.AdvanceProgress();

			} // for-loop

		} // DeleteFiles()

	} // class

} // namespace
