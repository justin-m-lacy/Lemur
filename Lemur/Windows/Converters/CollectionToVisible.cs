using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Displays an item only if the bound Collection is non-null and non-empty.
	/// </summary>
	public class CollectionToVisible : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			ICollection collection = value as ICollection;
			if( collection == null || collection.Count == 0 ) {
				return Visibility.Collapsed;
			}
			return Visibility.Visible;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotImplementedException();
		}

	} // class

} // namespace
