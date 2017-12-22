using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Name Contains", "Matches file based on the contents of the file name." )]
	public class NameContains : BaseCondition {

		/// <summary>
		/// Substring to find within the file name.
		/// </summary>
		private string _matchString;
		public string MatchString {
			get { return this._matchString; }
			set {
				this._matchString = value;
			}
		}

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			if( info.Name.Contains( _matchString ) ) {
				return base.IsMatch(true);
			}
			return base.IsMatch(false);

		}

	} // class

} // namespace
