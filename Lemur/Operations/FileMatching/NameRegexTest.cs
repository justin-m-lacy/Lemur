using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Matches files and directories whose name matches a regular expression.
	/// </summary>
	public class NameRegexTest : BaseCondition {

		private Regex _expr;
		public Regex TestExpression {
			get { return this._expr; }
			set {
				this._expr = value;
			}
		}

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			string testStr = info.Name;
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
