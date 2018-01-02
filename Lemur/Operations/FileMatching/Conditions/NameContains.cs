using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Name Contains", "Matches file based on the contents of the file name." )]
	[Serializable]
	public class NameContains : BaseCondition {

		/// <summary>
		/// If true, the full file path is searched for the match string.
		/// </summary>
		public bool FullPath { get => fullPath; set => fullPath = value; }
		private bool fullPath;

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

		

		public override bool IsMatch( FileSystemInfo info ) {

			string searchString = this.fullPath ? info.FullName : info.Name;

			if( searchString.Contains( _matchString ) ) {
				return base.IsMatch(true);
			}
			return base.IsMatch(false);

		}

	} // class

} // namespace
