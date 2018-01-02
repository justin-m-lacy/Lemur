using Lemur.Types;
using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Mode for testing files against a regular expression.
	/// NOTE: probably not very useful since with full-path expressions,
	/// it's easy to match any file name?
	/// </summary>
	[TypeConverter( typeof( EnumNameDescConverter) )]
	public enum FileNameSearchMode {

		[NameDesc( "Default", "By default only the file name is tested." )]
		Default =0,

		[NameDesc( "Name Only", "Only the file name will be tested.")]
		NameOnly = 1,

		[NameDesc( "Full Path", "The full File Path will be tested." )]
		FullPath = 2,

		[NameDesc( "Root Path", "Only the file path (and not the name) will be tested." )]
		RootPath = 4

	}

	/// <summary>
	/// Matches files and directories whose name matches a regular expression.
	/// </summary>
	[NameDesc( "Regular Expression", "Uses a regular expression to match file/directory names." )]
	[Serializable]
	public class RegexTest : BaseCondition {


		/// <summary>
		/// What portion of the file path should be tested for the regular expression.
		/// </summary>
		public FileNameSearchMode Mode {
			get => mode;
			set {

				mode = value;
			}
		}

		private FileNameSearchMode mode;

		/// <summary>
		/// The regular expression used in the match test.
		/// </summary>
		public Regex TestExpression {
			get { return this._expr; }
			set {
				this._expr = value;
			}
		}

		
		private Regex _expr;

		public override bool IsMatch( FileSystemInfo info ) {

			string testStr = null;
			switch( this.mode ) {
				case FileNameSearchMode.Default:
				case FileNameSearchMode.NameOnly:
					testStr = info.Name;
					break;
				case FileNameSearchMode.FullPath:
					testStr = info.FullName;
					break;

				case FileNameSearchMode.RootPath:
					testStr = Path.GetDirectoryName( info.FullName );
					break;
			}

			bool result = this._expr.IsMatch( testStr );

			return base.IsMatch( result );

		}

		public RegexTest( string pattern ) {

			this._expr = new Regex( pattern );

		}
		public RegexTest() {
			this._expr = new Regex( string.Empty );
		}

		public RegexTest( Regex exp ) {
			this._expr = exp;
		}

		public RegexTest( Regex exp, FileNameSearchMode mode ) {
			this._expr = exp;
			this.mode = mode;
		}

	} // class

} // namespace
