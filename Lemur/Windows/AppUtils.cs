using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Windows {

	public static class AppUtils {

		/// <summary>
		/// Attempt to open a list of files externally.
		/// All exceptions are caught by the function.
		/// </summary>
		/// <param name="paths"></param>
		public static async void OpenExternalAsync( string[] paths ) {

			await Task.Run( () => {

				foreach( string path in paths ) {
					if( string.IsNullOrEmpty( path ) ) {
						continue;
					}
					if( !File.Exists( path ) ) {
						continue;
					}
					try {
						System.Diagnostics.Process.Start( path );
					} catch( Exception ) {
					}
				}

			}

			);

		}

		/// <summary>
		/// Attempts to open a file in the explorer. All Exceptions are caught and ignored.
		/// </summary>
		/// <param name="path"></param>
		public static async Task<bool> OpenExternalAsync( string path ) {

			return await Task.Run( () => {
				if( string.IsNullOrEmpty( path ) ) {
					return false;
				}
				try {
					System.Diagnostics.Process.Start( path );
				} catch( Exception ) {
					return false;
				}
				return true;

			} );

		}

		/// <summary>
		// Attempts to show a file in the System Explorer. All exceptions are ignored.
		/// </summary>
		/// <param name="path"></param>
		public static async void ShowExternalAsync( string path ) {

			await Task.Run( () => {

				if( string.IsNullOrEmpty( path ) ) {
					return;
				}
				try {

					if( File.Exists( path ) ) {
						System.Diagnostics.Process.Start( Path.GetDirectoryName( path ) );
					} else if( Directory.Exists( path ) ) {
						System.Diagnostics.Process.Start( path );
					}

				} catch( Exception ) {
				}

			} );

		} // ShowExternalAsync()

	} // class

} // namespace
