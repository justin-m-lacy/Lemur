using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	public class StringToRegex : IValueConverter {

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value == null ) {
				return string.Empty;
			}
			Regex exp = value as Regex;
			if( exp != null ) {
				return exp.ToString();
			}
			throw new ArgumentException( "Value must be a Regular Expression." );

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( targetType != typeof( Regex ) ) {
				throw new ArgumentException( "Target must be a Regular Expression." );
			}
			if( value == null ) {
				return null;
			}
			string str_exp = value as string;
			if( str_exp != null ) {
				return new Regex( str_exp );
			}

			Regex exp = value as Regex;
			if( exp != null ) {
				return exp;
			}

			throw new ArgumentException( "Value should be a string or regular expression." );

		}

	} // class

} // namespace
