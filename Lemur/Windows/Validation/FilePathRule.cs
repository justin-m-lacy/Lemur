using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Windows.Controls;
using System.Globalization;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Validation Rule that ensures the name is a valid file path. (Not necessarily existing.)
	/// </summary>
	public class FilePathRule : ValidationRule {

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			string text = value.ToString();
			if( string.IsNullOrEmpty( text ) ) {
				return new ValidationResult( false, "Cannot convert value to a valid file path." );
			}

			char[] illegal = Path.GetInvalidPathChars();

			int len = text.Length;
			for( int i = 0; i < len; i++ ) {

				char c = text[i];
				if( illegal.Contains( c ) ) {
					return new ValidationResult( false, c + " is not allowed in a file path." );
				}

			} //

			return new ValidationResult( true, null );

		}

	} // class

} // namespace