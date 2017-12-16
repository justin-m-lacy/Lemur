using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Inverts a boolean value before binding.
	/// </summary>
	public class InvertBoolConverter : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
			return !(bool)value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			return !(bool)value;
		}

	} // class

} // namespace
