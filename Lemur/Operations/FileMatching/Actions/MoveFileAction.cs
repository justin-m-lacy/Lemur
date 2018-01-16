using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Move Files", "Moves the target files or directories to a new location." )]
	[Serializable]
	public class MoveFileAction : FileActionBase {

		public MoveFileAction() {}

		public MoveFileAction( string dest ) {
			this._destination = dest;
		}

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
		override public bool Run( FileSystemInfo info ) {

			string new_path = Path.Combine( this._destination, info.Name );

			if( info is FileInfo ) {

				File.Move( info.FullName, new_path );
				return true;

			}
			if( info is DirectoryInfo ) {

				Directory.Move( info.FullName, new_path );
				return true;

			}

			return false;

		}

	} // class

} // namespace
