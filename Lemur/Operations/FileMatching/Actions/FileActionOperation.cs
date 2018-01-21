using Lemur.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	/// <summary>
	/// Performs a sequence of actions on the given list of file/directory targets.
	/// </summary>
	[Serializable]
	public class FileActionOperation : ProgressOperation {

		#region PROPERTIES

		/// <summary>
		/// Conditions that files or directories must match in order
		/// to be included in a match operation.
		/// </summary>
		public ICollection<IFileAction> Actions {
			get { return this._actions; }
			set { this._actions = value; }
		}
		private ICollection<IFileAction> _actions;

		/// <summary>
		/// List of Exceptions encountered during the search.
		/// </summary>
		private readonly List<Exception> errorList = new List<Exception>();
		public Exception[] ErrorList {
			get { return this.errorList.ToArray(); }
		}

		/// <summary>
		/// Files or Directories which are the targets of the Operation.
		/// </summary>
		public ICollection<FileSystemInfo> Targets {
			get { return this.targets; }
			set { this.targets = value; }
		}
		private ICollection<FileSystemInfo> targets = new List<FileSystemInfo>();

		#endregion

		public override void Run() {

			this.errorList.Clear();
			this.ResetProgress();

			if( this._actions == null ) {

				Console.WriteLine( "FileActionOperation: NO ACTIONS FOUND" );
			} else if( this.targets == null ) {
				Console.WriteLine( "FileActionOperation: NO TARGETS FOUND" );

			} else {

				Console.WriteLine( "FileActionOperation: Run()" );
				this.RunActions();

			}

			this.OperationComplete();

		}

		private void RunActions() {

			int fileCount = this.targets.Count;
			this.SetMaxProgress( this._actions.Count * fileCount );

			foreach( IFileAction action in this._actions ) {

				if( action.RunOnce ) {

					try {

						action.Run( new FileInfo( "/" ) );

					} catch( Exception e ) {
						this.errorList.Add( e );
					}
					this.AdvanceProgress( fileCount );

				} else {

					foreach( FileSystemInfo info in targets ) {

						try {
							action.Run( info );
						} catch( Exception e ) {
							this.errorList.Add( e );
						}
						this.AdvanceProgress();

					} //

				}

			} // action loop.

		}

	} // class

} // namespace