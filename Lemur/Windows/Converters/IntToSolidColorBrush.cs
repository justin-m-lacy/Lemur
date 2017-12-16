using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Converts from a 32 bit ARGB value to a SolidColorBrush.
	/// </summary>
	public class IntToSolidColorBrush : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value is int ) {

				int color = (int)value;
				Color c = Color.FromArgb(
					(byte)( color >> 24 ), (byte)( 0xFF & ( color >> 16 ) ), (byte)( 0xFF & ( color >> 8 ) ), (byte)( 0xFF & color ) );

				return new SolidColorBrush( c );

			}
			return new SolidColorBrush();

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value is SolidColorBrush ) {

				Color c = ( (SolidColorBrush)value ).Color;

				return ( c.A << 24 ) + ( c.R << 16 ) + ( c.G << 8 ) + c.B;

			}
			return 0;

		}

	} // class

} // namespace
