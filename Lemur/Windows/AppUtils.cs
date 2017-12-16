using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Windows {

	public static class AppUtils {

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
