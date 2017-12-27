using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Matches files and directories whose name matches a regular expression.
	/// </summary>
	[NameDesc( "Name Regular Expression", "Match with regular expression on file name." )]
	[Serializable]
	public class NameRegexTest : BaseCondition {

		/// <summary>
		/// If true, the full file path is searched for the match string.
		/// </summary>
		public bool FullPath { get => fullPath; set => fullPath = value; }
		private bool fullPath;

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

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			string testStr = this.fullPath ? info.FullName : info.Name;

			bool result = this._expr.IsMatch( testStr );

			return base.IsMatch( result );

		}

		public NameRegexTest( string pattern ) {

			this._expr = new Regex( pattern );

		}
		public NameRegexTest() {
			this._expr = new Regex( string.Empty );
		}

		public NameRegexTest( Regex exp ) {
			this._expr = exp;
		}

	} // class

} // namespace
