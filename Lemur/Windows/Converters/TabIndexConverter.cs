using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Converts from a Tab name to a selected tab index.
	/// </summary>
	public class TabNameConverter : DependencyObject, IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( Object.Equals( value, null ) ) {
				return 0;
			}
			if( value is int ) {
				return (int)value;
			}
			if( value is string ) {

				TabControl control = parameter as TabControl;
				if( control != null ) {
	
					int index = 0;
					foreach( var child in control.Items ) {
						//Console.WriteLine( "checking for matching tab: " + child.ToString() );
						TabItem childTab = child as TabItem;
						
						if( childTab != null && childTab.Name == (string)value ) {
							return index;
						}
						index++;

					} // foreaech child

				} // TabControl

			}

			return 0;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value is int ) {
				
				int index = (int)value;
				return GetTabName( index, parameter as TabControl );

			} else if( value is int? ) {

				//Console.WriteLine( "nullable int" );
				int? nullint = (int?)value;
				if( nullint.HasValue ) {
					return GetTabName( nullint.Value, parameter as TabControl );
				}

			}

			return string.Empty;

		}

		private string GetTabName( int index, TabControl control ) {

			if( control == null ) {
				return index.ToString();
			}
			var child = control.Items.GetItemAt( index ) as TabItem;
			if( child != null ) {
				return child.Name;
			}
			return string.Empty;

		}

	} // class

} // namespace