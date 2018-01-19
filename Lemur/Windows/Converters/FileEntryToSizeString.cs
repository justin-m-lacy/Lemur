using Lemur.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Converts from FileSystemInfo to size string.
	/// </summary>
	public class FileEntryToSizeString : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			FileInfo fInfo = value as FileInfo;
			if( fInfo != null ) {
				return DataSize.ToDataString( fInfo.Length );
			}

			DirectoryInfo dInfo = value as DirectoryInfo;
			if( dInfo != null ) {
				return "-";
			}
			throw new ArgumentException( "Value must be a FileSystemInfo object" );
		} //

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotSupportedException();
		}

	} // class

} // namespace
