using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	public class NameContainsCondition : BaseCondition {

		/// <summary>
		/// Substring to find within the file name.
		/// </summary>
		private string _nameSubstring;
		public string NameSubstring {
			get { return this._nameSubstring; }
			set {
				this._nameSubstring = value;
			}
		}

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			if( info.Name.Contains( _nameSubstring ) ) {
				return base.IsMatch(true);
			}
			return base.IsMatch(false);

		}

	} // class

} // namespace
