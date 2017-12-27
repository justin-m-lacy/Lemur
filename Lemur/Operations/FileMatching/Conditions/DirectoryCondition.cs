using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lemur.Operations.FileMatching {

	[Serializable]
	public abstract class DirectoryCondition : BaseCondition {

		private bool _haltRecursionOnFail;
		/// <summary>
		/// If true, a file match search should not check subdirectories of this directory,
		/// if this match condition is not met.
		/// By default, in a recursive search, subdirectories are still searched, even
		/// if the current directory is not a match.
		/// </summary>
		public bool HaltOnFail {
			get { return this._haltRecursionOnFail; }
			set {
				this._haltRecursionOnFail = value;
			}
		}

    } // class

} // namespace
