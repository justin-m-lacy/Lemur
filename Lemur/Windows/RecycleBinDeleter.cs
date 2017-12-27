using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static Lemur.Windows.NativeMethods;

namespace Lemur.Windows {

	/// <summary>
	/// 
	/// !!!NOTE: CODE NOT ORIGINALLY BY LEMUR.
	/// 
	/// ORIGIN: SyncTrayzor
	/// via: http://stackoverflow.com/a/3282481/1086121
	/// 
	/// Assumed public from public posting with no attribution requirements.
	/// 
	/// </summary>
	public static class RecycleBinDeleter {

		public static bool EmptyRecycleBin( RecycleFlag flags=0 ) {

			try {

				int result = NativeMethods.SHEmptyRecycleBin( IntPtr.Zero, null, flags );
				return ( result == 0 );
	
			} catch( Exception ) {
				return false;
			}

		}

		/// <summary>
		/// Send file to recycle bin.
		/// </summary>
		/// <param name="path">Location of directory or file to recycle</param>
		public static bool Delete( string path ) {

			try {
				var fs = new NativeMethods.SHFILEOPSTRUCT {
					wFunc = NativeMethods.FileOperationType.FO_DELETE,
					pFrom = path + '\0' + '\0',
					fFlags = NativeMethods.FileOperationFlags.FOF_ALLOWUNDO |
							 NativeMethods.FileOperationFlags.FOF_SILENT |
							 NativeMethods.FileOperationFlags.FOF_NOCONFIRMATION |
							 NativeMethods.FileOperationFlags.FOF_AUTODELETEWARNING,
				};
				int result = NativeMethods.SHFileOperation( ref fs );
				if( result != 0 ) {
					//Logger( String.Format( "Delete file operation on {0} failed with error {1}", path, result ) );
					return false;
				}
				return true;

			} catch( Exception ) {
				return false;
			}

		}

	} // class

} // namespace
