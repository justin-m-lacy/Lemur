using Lemur.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using Lemur.Utils;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Converts a long value to a DataSize string. (i.e. kb, mb, etc.)
	/// </summary>
	public class LongToDataString : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( !DataUtils.IsNumber(value) ) {
				throw new ArgumentException( "Cannot convert from " + value.GetType().ToString() );
			}

			return DataSize.ToDataString( (long)value );

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

			/// Equals() prevents boxing.
			if( Equals(value, null) || !( value is string ) ) {
				throw new ArgumentException( "Cannot convert type to long." );
			}

			return DataSize.GetAsBytes( (string)value );

		}

	} // class

} // namespace
