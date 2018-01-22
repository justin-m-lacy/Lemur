using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Name Replace", "Replaces characters in a target file or directory with a given replacement string." )]
	[Serializable]
	public class NameReplace : FileActionBase {

		private string searchString;
		private string replaceString;

		public string SearchString { get => searchString; set => searchString = value; }
		public string ReplaceString { get => replaceString; set => replaceString = value; }

		public NameReplace() {
		}

		public NameReplace( string searchFor, string replace ) {

			this.searchString = searchFor;
			this.replaceString = replace;
		}

		override public FileActionResult Run( FileSystemInfo info ) {

			string newName = info.Name.Replace( searchString, replaceString );
			string path = Path.GetDirectoryName( info.FullName );

			string newPath = Path.Combine( path, newName );

			// rename
			if( info is FileInfo ) {
				File.Move( info.FullName, newPath );
				return new FileActionResult( true, new FileInfo(newPath) );
			}

			if( info is DirectoryInfo ) {
				Directory.Move( info.FullName, newPath );
				return new FileActionResult( true, new DirectoryInfo( newPath ) );
			}

			return new FileActionResult(false);
		}

	} // class

} // namespace
