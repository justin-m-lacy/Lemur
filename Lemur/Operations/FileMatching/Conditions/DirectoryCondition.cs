using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lemur.Operations.FileMatching {

	public abstract class DirectoryCondition : BaseCondition {

		private bool _recursive;
		/// <summary>
		/// Whether to apply the condition to directories recursively.
		/// </summary>
		public bool Recursive {
			get { return this._recursive; }
			set {
				this._recursive = value;
			}
		}

		/*public bool IsMatch( FileSystemInfo info ) {
		}*/

    } // class

} // namespace
