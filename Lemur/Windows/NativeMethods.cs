using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Lemur.Windows {

	public static class NativeMethods {

		#region RECYCLE BIN DELETE

		/// !!!NOTE: RECYCLE CODE NOT BY LEMUR.
		/// 
		/// ORIGIN: SyncTrayzor
		/// via: http://stackoverflow.com/a/3282481/1086121
		/// 
		/// Assumed public from public post with no attribution requirements.


		/// <summary>
		/// Possible flags for the SHFileOperation method.
		/// </summary>
		[Flags]
		public enum FileOperationFlags : ushort {
			/// <summary>
			/// Do not show a dialog during the process
			/// </summary>
			FOF_SILENT = 0x0004,
			/// <summary>
			/// Do not ask the user to confirm selection
			/// </summary>
			FOF_NOCONFIRMATION = 0x0010,
			/// <summary>
			/// Delete the file to the recycle bin.  (Required flag to send a file to the bin
			/// </summary>
			FOF_ALLOWUNDO = 0x0040,
			/// <summary>
			/// Do not show the names of the files or folders that are being recycled.
			/// </summary>
			FOF_SIMPLEPROGRESS = 0x0100,
			/// <summary>
			/// Surpress errors, if any occur during the process.
			/// </summary>
			FOF_NOERRORUI = 0x0400,
			/// <summary>
			/// Warn if files are too big to fit in the recycle bin and will need
			/// to be deleted completely.
			/// </summary>
			FOF_AUTODELETEWARNING = 0x4000,
		}

		/// <summary>
		/// File Operation Function Type for SHFileOperation
		/// </summary>
		public enum FileOperationType : uint {
			/// <summary>
			/// Move the objects
			/// </summary>
			FO_MOVE = 0x0001,
			/// <summary>
			/// Copy the objects
			/// </summary>
			FO_COPY = 0x0002,
			/// <summary>
			/// Delete (or recycle) the objects
			/// </summary>
			FO_DELETE = 0x0003,
			/// <summary>
			/// Rename the object(s)
			/// </summary>
			FO_RENAME = 0x0004,
		}

		/// <summary>
		/// SHFILEOPSTRUCT for SHFileOperation from COM
		/// </summary>
		[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
		public struct SHFILEOPSTRUCT {

			public IntPtr hwnd;
			[MarshalAs( UnmanagedType.U4 )]
			public FileOperationType wFunc;
			public string pFrom;
			public string pTo;
			public FileOperationFlags fFlags;
			[MarshalAs( UnmanagedType.Bool )]
			public bool fAnyOperationsAborted;
			public IntPtr hNameMappings;
			public string lpszProgressTitle;

		}

		[DllImport( "shell32.dll", CharSet = CharSet.Auto )]
		public static extern int SHFileOperation( ref SHFILEOPSTRUCT FileOp );

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pBC">A pointer to the IBindCtx interface. Can be set to NULL.</param>
		/// <param name="pwzUrl">A pointer to a string value that contains the URL of the data. Can be set to NULL if pBuffer contains the data to be sniffed. </param>
		/// <param name="pBuffer">A pointer to the buffer that contains the data to be sniffed. Can be set to NULL if pwzUrl contains a valid URL. </param>
		/// <param name="cbSize">An unsigned long integer value that contains the size of the buffer. </param>
		/// <param name="pwzMimeProposed">A pointer to a string value that contains the proposed MIME type. This value is authoritative if type cannot be determined from the data. If the proposed type contains a semi-colon (;) it is removed. This parameter can be set to NULL.</param>
		/// <param name="dwMimeFlags"></param>
		/// <param name="ppwzMimeOut">The address of a string value that receives the suggested MIME type.</param>
		/// <param name="dwReserved">Reserved. Must be set to 0.</param>
		/// <returns></returns>
		[DllImport( @"urlmon.dll", CharSet = CharSet.Auto )]
		internal extern static System.UInt32 FindMimeFromData(
				IntPtr pBC,
				[MarshalAs( UnmanagedType.LPWStr )] System.String pwzUrl,
				[MarshalAs( UnmanagedType.LPArray )] byte[] pBuffer,
				uint cbSize,
				[MarshalAs( UnmanagedType.LPWStr )] System.String pwzMimeProposed,
				uint dwMimeFlags,
				out IntPtr ppwzMimeOut,
				uint dwReserved

			);

	} // class

} // namespace