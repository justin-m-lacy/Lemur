using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[Serializable]
	public abstract class FileActionBase : IFileAction {

		private bool _runOnce;
		public bool RunOnce { get => _runOnce; set => _runOnce = value; }

		virtual public bool Run( FileSystemInfo info ) {
			return false;
		}

		/// <summary>
		/// Runs the action on every File or Directory in the list.
		/// If RunOnce is true, it runs the action once using the first entry in the Enumeration.
		/// If the enumeration is empty, the root directory is used.
		/// </summary>
		/// <param name="fileList"></param>
		/// <returns></returns>
		virtual public bool Run( IEnumerable<FileSystemInfo> fileList ) {

			bool success = true;

			if( this._runOnce ) {

				using( IEnumerator<FileSystemInfo> files = fileList.GetEnumerator() ) {

					if( files.MoveNext() ) {

						return this.Run( files.Current );

					} else {
						return this.Run( new FileInfo( "/" ) );
					}

				}

			} else {

				foreach( FileSystemInfo info in fileList ) {

					if( !this.Run( info ) ) {
						success = false;
					}

				}

			}

			return success;

		}

	} // class

} // namespace
