using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Returns Visibility.Visible only when ICommand.CanExecute() returns true.
	/// At the moment, null is always passed to the ICommand.CanExecute method.
	/// </summary>
	public class CommandToVisible : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			ICommand cmd = value as ICommand;
			if( cmd != null ) {

				if( cmd.CanExecute( null ) ) {
					return Visibility.Visible;
				}

			}
			return Visibility.Collapsed;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotSupportedException();
		}

	} // class

} // namespace
