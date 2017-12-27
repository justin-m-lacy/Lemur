using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Move Files", "Moves the target files or directories to a new location." )]
	[Serializable]
	public class MoveFileAction : IFileAction {

		private string _destination;
		public string Destination {
			get {
				return this._destination;
			}
			set {
				this._destination = value;
			}
		}

		/// <summary>
		/// TODO: check permissions, etc?
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public bool Run( FileSystemInfo info ) {

			if( info is FileInfo ) {

				File.Move( info.Name, this._destination );
				return true;

			}
			if( info is DirectoryInfo ) {

				Directory.Move( info.Name, this._destination );
				return true;

			}

			return false;

		}

	} // class

} // namespace
