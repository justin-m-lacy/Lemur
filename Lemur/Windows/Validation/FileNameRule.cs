using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.IO;
using System.Linq;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Validation Rule that ensures the name is a valid file name. (Not necessarily existing.)
	/// </summary>
	public class FileNameRule : CustomValidationRule {

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			string text = value.ToString();
			if( string.IsNullOrEmpty( text ) ) {
				return new ValidationResult( false, "Cannot convert value to valid file name." );
			}

			char[] illegal = Path.GetInvalidFileNameChars();

			int len = text.Length;
			for( int i = 0; i < len; i++ ) {

				char c = text[i];
				if( illegal.Contains( c ) ) {
					return new ValidationResult( false, c + " is not a valid character." );
				}

			} //

			return new ValidationResult( true, null );

		} //

	} // class

}