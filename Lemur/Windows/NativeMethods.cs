using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Lemur.Windows {

	public static class NativeMethods {

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
