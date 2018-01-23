using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Move Files", @"Moves the target files or directories to a new location." )]
	[Serializable]
	public class MoveFileAction : FileActionBase {

		/// <summary>
		/// If true, the action will attempt to create any non-existing
		/// directories specified in the Move-Path.
		/// </summary>
		public bool CreateDirs {

			get { return this._createDirs; }
			set { this._createDirs = value; }

		}
		private bool _createDirs = true;

		/// <summary>
		/// Overwrite any existing files.
		/// TODO
		/// </summary>
		public bool Overwrite {
			get => this._overwrite;
			set => this._overwrite = value;
		}

		/// <summary>
		/// Returns true if the current destination is a relative path.
		/// </summary>
		public bool IsRelative {
			get { return !this._isAbsolute; }
		}
		public bool IsAbsolute {
			get { return this._isAbsolute; }
		}

		private bool _overwrite;
		private bool _isAbsolute;

		/// <summary>
		/// The destination directory of the move operation. This can be an absolute
		/// or relative path.
		/// </summary>
		public string Destination {
			get {
				return this._destination;
			}
			set {

				this._destination = value;
				this._isAbsolute = Path.IsPathRooted( value );

			}
		}
		private string _destination;

		public MoveFileAction() {}

		public MoveFileAction( string dest ) {
			this._destination = dest;
		}

		
		/// <summary>
		/// TODO: check permissions, etc?
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		override public FileActionResult Run( FileSystemInfo info ) {

			string new_dir;

			if( this._isAbsolute ) {
				new_dir = this._destination;
			} else {
				new_dir = Path.Combine( Path.GetDirectoryName( info.FullName ), this._destination );
			}

			/// TODO: This operation could be a waste since it might only need to run once.
			/// But if the destination is a relative path, even having a pre-run method
			/// wouldn't be enough to confirm path existence.
			/// Might have a pre-run do absolute paths, and relative paths check every file.
			if( this.CreateDirs ) {
				Directory.CreateDirectory( new_dir );
			}

			string new_path = Path.Combine( new_dir, info.Name );

			if( info is FileInfo ) {

				File.Move( info.FullName, new_path );
				return new FileActionResult( true, new FileInfo(new_path) );

			}
			if( info is DirectoryInfo ) {

				Directory.Move( info.FullName, new_path );
				return new FileActionResult( true, new DirectoryInfo(new_path) );

			}

			return new FileActionResult(false);

		}

	} // class

} // namespace
