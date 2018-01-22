using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Regex Name Replace", "Uses a regular expression to replace characters in a file name." )]
	[Serializable]
	public class RegexReplace : FileActionBase {

		public Regex ReplaceRule { get => replaceRule; set => replaceRule = value; }
		private Regex replaceRule;

		public string ReplaceString { get => replaceString; set => replaceString = value; }
		private string replaceString;

		public RegexReplace() {
		} //

		public RegexReplace( Regex rule, string replace ) {

			this.replaceRule = rule;
			this.replaceString = replace;

		}

		override public FileActionResult Run( FileSystemInfo info ) {

			if( replaceRule == null ) {
				return new FileActionResult( false);
			}

			string newName = this.replaceRule.Replace( info.Name, this.replaceString );
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
