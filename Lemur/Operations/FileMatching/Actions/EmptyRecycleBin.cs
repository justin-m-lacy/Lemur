using Lemur.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Lemur.Windows;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Empty Recycle Bin", "Empties the Recycle Bin. This action cannot be undone." )]
	[Serializable]
	public class EmptyRecycleBin : FileActionBase {

		/// <summary>
		/// Whether to show a dialog confirmation while emptying.
		/// False by default.
		/// </summary>
		public bool ShowConfirm { get => showConfirm; set => showConfirm = value; }
		private bool showConfirm;

		/// <summary>
		/// Whether to show a progress dialog while the recycle bin
		/// is being emptied.
		/// false by default.
		/// </summary>
		public bool ShowProgress { get => showProgress; set => showProgress = value; }
		private bool showProgress;

		public EmptyRecycleBin() {
		}

		public EmptyRecycleBin( bool confirm, bool showProgress = false ) {

			this.showConfirm = confirm;
			this.showProgress = showProgress;

		}

		override public bool Run( FileSystemInfo info ) {

			NativeMethods.RecycleFlag flags = 0;
			if( !showConfirm ) {
				flags |= NativeMethods.RecycleFlag.SHERB_NOFCONFIRMATION;
			}
			if( !showProgress ) {
				flags |= NativeMethods.RecycleFlag.SHERB_NOPROGRESSUI;
			}

			return RecycleBinDeleter.EmptyRecycleBin( flags );

		}

	} // class

} // namespace