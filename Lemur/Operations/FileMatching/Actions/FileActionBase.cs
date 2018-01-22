using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[Serializable]
	public abstract class FileActionBase : IFileAction {

		private bool _runOnce;
		public bool RunOnce { get => _runOnce; set => _runOnce = value; }

		virtual public FileActionResult Run( FileSystemInfo info ) {
			return new FileActionResult( false );
		}

		/// <summary>
		/// Runs the action on every File or Directory in the list.
		/// If RunOnce is true, it runs the action once using the first entry in the Enumeration.
		/// If the enumeration is empty, the root directory is used.
		/// </summary>
		/// <param name="fileList"></param>
		/// <returns></returns>
		virtual public FileActionResult[] Run( IEnumerable<FileSystemInfo> fileList ) {

			if( this._runOnce ) {

				using( IEnumerator<FileSystemInfo> files = fileList.GetEnumerator() ) {

					if( files.MoveNext() ) {

						return new[] { this.Run( files.Current ) };

					} else {
						return new[] { this.Run( new FileInfo( "/" ) ) };
					}

				}

			} else {

				List<FileActionResult> results = new List<FileActionResult>();

				foreach( FileSystemInfo info in fileList ) {

					results.Add( this.Run( info ) );

				}
				return results.ToArray();

			}

		}

	} // class

} // namespace
