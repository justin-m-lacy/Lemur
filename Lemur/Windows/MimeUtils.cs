using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Windows {

	static public class MimeUtils {

		public const string Audio = "audio";
		public const string Image = "image";
		public const string Video = "video";
		public const string Text = "text";
		public const string Unknown = "unknown";

		private const char Period = '.';

		private static string[] mediaExtensions = { "mpg", "mp3", "mp4", "m4a", "avi", "flv", "wma", "wmv", "wav", "mpeg", "webm", "mov" };
		private static string[] imageExtensions = { "png", "jpg", "jpeg", "gif", "bmp" };
		private static string[] textExtentions = { "txt", "text", "xml", "html", "htm",
													"cc", "c", "cs", "css", "php", "config", "ini", "cfg", "java", "js",
													"as", "xaml", "cpp", "h", "perl", "py" };

		/// <summary>
		/// Internet Explorer 9. Returns image/png and image/jpeg instead of 
		/// image/x-png and image/pjpeg.
		/// </summary>
		private const uint FMFD_RETURNUPDATEDIMGMIMES = 0x20;

		/// <summary>
		/// The return value which indicates that the operation completed successfully.
		/// </summary>
		private const uint S_OK = 0;

		/// <summary>
		/// The zero (0) value for Reserved parameters.
		/// </summary>
		private const uint RESERVED = 0;

		/// <summary>
		/// Unknown MIME type.
		/// </summary>
		public const string MIME_UNKNOWN = "unknown/unknown";

		private const int HEADER_LEN = 256;

		public const char MIME_SLASH = '/';



		/// <summary>
		/// Returns the specific format of a mime type.
		/// (jpg,mp3,png, etc.)
		/// </summary>
		/// <param name="mime"></param>
		/// <returns></returns>
		public static string GetMimeFormat( string mime ) {

			if( string.IsNullOrEmpty( mime ) ) {
				return string.Empty;
			}
			int index = mime.IndexOf( MIME_SLASH );
			if( index < 0 ) {
				return string.Empty;
			}
			return mime.Substring( index + 1 );

		}

		/// <summary>
		/// Returns the root portion of a MIME file type.
		/// </summary>
		/// <param name="mime"></param>
		/// <returns></returns>
		public static string GetMimeRoot( string mime ) {

			if( string.IsNullOrEmpty( mime ) ) {
				return string.Empty;
			}
			int index = mime.IndexOf( MIME_SLASH );
			if( index >= 0 ) {
				return mime.Substring( 0, index );
			}

			return mime;
		}

		/// <summary>
		/// Gets the assumed MIME type from the given file extension.
		/// </summary>
		/// <param name="ext"></param>
		/// <returns></returns>
		public static string MimeFromExt( string ext ) {

			if( !string.IsNullOrEmpty( ext ) ) {

				if( ext[0] == MimeUtils.Period ) {
					ext = ext.Substring( 1 );
				}

			}
			if( mediaExtensions.Contains( ext ) ) {
				return Video + MIME_SLASH + ext;
			}
			if( imageExtensions.Contains( ext ) ) {
				return Image + MIME_SLASH + ext;
			}
			if( textExtentions.Contains( ext ) ) {
				return Text + MIME_SLASH + ext;
			}
			return Unknown + MIME_SLASH + ext;

		}

		/// <summary>
		/// Returns the MIME associated with the given file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetFileMime( string filePath, string ext=null ) {

			if( string.IsNullOrEmpty( filePath ) ) {
				return MIME_UNKNOWN;
			}
			if( string.IsNullOrEmpty( ext ) ) {
				ext = Path.GetExtension( filePath );
			}

			string proposed = MimeFromExt( ext );

			try {

				byte[] header;

				using( FileStream stream = new FileStream( filePath, FileMode.Open, FileAccess.Read ) ) {

					if( stream.Length >= HEADER_LEN ) {
						header = new byte[HEADER_LEN];
					} else {
						header = new byte[stream.Length];
					}
					stream.Read( header, 0, header.Length );

					return MimeUtils.GetMime( header, proposed );
				}

			} catch( Exception ) {
				return MIME_UNKNOWN;
			}

		}

		/// <summary>
		/// Returns the MIME associated with the given header.
		/// </summary>
		/// <param name="header"></param>
		/// <returns></returns>
		public static string GetMime( byte[] header, string proposed=null) {

			try {

				IntPtr mimeTypePtr;
				uint result = NativeMethods.FindMimeFromData( IntPtr.Zero,
												null,
												header,
												(uint)header.Length,
												proposed,
												FMFD_RETURNUPDATEDIMGMIMES,
												out mimeTypePtr,
												RESERVED );
				if( result != S_OK ) {
					return MIME_UNKNOWN;
				}

				//IntPtr mimeTypePtr = new IntPtr( mimetype );
				string mime = Marshal.PtrToStringUni( mimeTypePtr );
				Marshal.FreeCoTaskMem( mimeTypePtr );
				return mime;
	
			} catch {

				return MIME_UNKNOWN;
			}

		}

		// NOTE: IntPtr to string. Then free memory.
		//IntPtr mimeTypePtr = new IntPtr( mimetype );
		//string mime = Marshal.PtrToStringUni( mimeTypePtr );
		//Marshal.FreeCoTaskMem(mimeTypePtr );

	} //

} // namespace
