using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace Lemur.Windows.Validation {

	public class IntValidationRule : CustomValidationRule {

		public IntValidationRule() {

			this.DefaultErrorMessage = "Value must be an integer.";

		}

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			if( value == null ) {

				return Result( false );

			} else if( value is string ) {

				int res;
				if( int.TryParse( (string)value, out res ) ) {
					return Result( true );
				}

			} else if( value is int || value is long || value is uint || value is ulong ) {
				// close enough.
				return Result( true );
			}

			return Result( false );

		}

	} // class

} // namespace
