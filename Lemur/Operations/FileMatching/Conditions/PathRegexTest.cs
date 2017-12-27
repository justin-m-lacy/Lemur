using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Matches a File or Directory whose full file path matches a regular expression.
	/// </summary>
	[NameDesc( "Path Regular Expression", "Match with regular expression on the file path.")]
	[Serializable]
	public class PathRegexTest : BaseCondition {

		private Regex _expr;
		public Regex TestExpression {
			get { return this._expr; }
			set {
				this._expr = value;
			}
		}

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			string testString = info.FullName;
			bool result = this._expr.IsMatch( testString );

			return base.IsMatch( result );

		}

		public PathRegexTest( string stringExp ) {

			this._expr = new Regex( stringExp );

		} //

		public PathRegexTest( Regex exp ) {
			this._expr = exp;
		}

		public PathRegexTest() {
			this._expr = new Regex( string.Empty );
		}

	} // class

} // namespace
