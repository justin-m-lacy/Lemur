using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	public struct FileActionResult {

		/// <summary>
		/// Whether the action succeeded.
		/// </summary>
		public bool success;

		/// <summary>
		/// Optional File to replace the current element with.
		/// </summary>
		public FileSystemInfo fileReplace;

		public FileActionResult( bool success, FileSystemInfo replace ) {
			this.success = success;
			this.fileReplace = replace;
		}

		public FileActionResult( bool success ) {

			this.success = success;
			this.fileReplace = null;

		}

		public FileActionResult( FileSystemInfo replace ) {
			success = true;
			this.fileReplace = replace;
		}


    } // class

} // namespace
