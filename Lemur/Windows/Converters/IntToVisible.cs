using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Sets the Visibility of a UIElement to Visible when the integer parameter
	/// is greater than zero. This is used to bind to Collection.Count, to only display
	/// a UIElement when the collection has items.
	/// </summary>
	public class IntToVisible : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value == null ) {
				return Visibility.Collapsed;
			}

			if( value is int ) {
	
				int count = (int)value;
				if( count > 0 ) {
					return Visibility.Visible;
				} else {
					return Visibility.Collapsed;
				}

			}
			throw new ArgumentException( "Value must be an integer." );

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotSupportedException();
		}

	} // class

} // namespace
