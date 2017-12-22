using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lemur.Types;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Matches a file based on its file extension. Directories are ignored.
	/// </summary>
	[NameDesc( "File Extension", "Matches files based on file extension." )]
	public class ExtensionCondition : BaseCondition {

		private string[] _extensions;
		public string[] Extensions {
			get { return this._extensions; }
			set { this._extensions = value; }
		}

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			if( this._extensions == null || this._extensions.Length == 0 ) {
				return base.IsMatch(false);
			}

			string ext = info.Extension;
			if( ext == null ) {
				return base.IsMatch( false );
			}

			foreach( string s in this._extensions ) {

				if( s == null ) {
					continue;
				}
				if( s == ext ) {
					// extension match.
					return base.IsMatch( true );
				}

			} //

			// no match.
			return base.IsMatch( false );

		} //

	} // class

} // namespace