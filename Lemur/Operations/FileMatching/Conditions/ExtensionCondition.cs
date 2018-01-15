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
	[Serializable]
	public class ExtensionCondition : BaseCondition {

		private const char LEADING_DOT = '.';

		private string[] _extensions;
		public string[] Extensions {
			get { return this._extensions; }

			set {
				this._extensions = value;
				PrefixDots();

			}

		}

		/// <summary>
		/// Extensions reported by FileInfo.Extension include the leading periods,
		/// so they must be added to the extensions for testing.
		/// </summary>
		private void PrefixDots() {

			if( this._extensions == null ) {
				return;
			}

			for( int i = this._extensions.Length - 1; i >= 0; i-- ) {

				string ext = this._extensions[i];
				if( !string.IsNullOrEmpty(ext) ) {

					if( ext[0] != LEADING_DOT ) {
						this._extensions[i] = LEADING_DOT + ext;
					}

				}

			} // for-loop.

		}

		public override bool IsMatch( FileSystemInfo info ) {

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