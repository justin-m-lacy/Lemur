using Lemur.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// NOTE: Converters can't do Validation. Exceptions are treated as runtime errors.
	/// </summary>
	public class DataSizeToString : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( ( value is DataSize ) ) {

				DataSize size = (DataSize)value;
				return size.ToString();
			}
			return DependencyProperty.UnsetValue;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

			DataSize size;
			if( DataSize.TryParse( (string)value, out size ) ) {
				return size;
			}

			return DependencyProperty.UnsetValue;

		}

	} // class

} // namespace
