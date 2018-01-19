using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Converts to true if the variable matches, false otherwise.
	/// </summary>
	public class StringMatchToBool : DependencyObject, IValueConverter {

		public static readonly DependencyProperty TargetStringProperty = DependencyProperty.Register( "TargetString", typeof( string ),
			typeof( StringMatchToBool ) );

		/// <summary>
		/// String to match in order for the Converter to return true.
		/// </summary>
		public string TargetString {
			get { return (string)this.GetValue( TargetStringProperty ); }
			set {
				if( value != this.TargetString ) {
					this.SetValue( TargetStringProperty, value );
				}
			}

		}

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( Object.Equals(value, null ) || !( value is string ) ) {
				return false;
			}

			string s = (string)value;
			if( s == TargetString ) {
				return true;
			}

			return false;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotSupportedException();
		}

	} // class

} // namespace
