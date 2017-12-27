using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Regex Name Replace", "Replaces characters in a file name according to the given regular expression." )]
	[Serializable]
	public class RegexReplace : IFileAction {

		private Regex replaceRule;
		private string replaceString;

		public Regex ReplaceRule { get => replaceRule; set => replaceRule = value; }
		public string ReplaceString { get => replaceString; set => replaceString = value; }

		public RegexReplace( Regex rule, string replace ) {

			this.replaceRule = rule;
			this.replaceString = replace;

		}

		public bool Run( FileSystemInfo info ) {

			string newName = this.replaceRule.Replace( info.Name, this.replaceString );
			string path = Path.GetDirectoryName( info.FullName );

			string newPath = Path.Combine( path, newName );

			// rename
			if( info is FileInfo ) {
				File.Move( info.FullName, newPath );
				return true;
			}

			if( info is DirectoryInfo ) {
				Directory.Move( info.FullName, newPath );
			}

			return false;

		}

	} // class

} // namespace
