using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Returns Visiblity.Visible if the bound object is not-null,
	/// and Visiblity.Collapsed otherwise.
	/// </summary>
	public class NonNullToVisible : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value == null ) {
				return Visibility.Collapsed;
			}
			if( value is string && ((string)value) == string.Empty) {
				return Visibility.Collapsed;
			}

			return Visibility.Visible;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotSupportedException();
		}

	} // class

} // namespace
