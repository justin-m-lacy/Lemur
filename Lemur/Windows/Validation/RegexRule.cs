using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Validate string inputs that conform to a given regular expression.
	/// </summary>
	public class RegexRule : CustomValidationRule {

		private Regex _exp;
		public Regex Expression {
			get { return this._exp; }
			set { this._exp = value; }
		}

		public RegexRule() {
		}

		public RegexRule( Regex exp ) {

			this._exp = exp;

		}

		public RegexRule( string pattern ) {

			this._exp = new Regex( pattern );
		}


		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			if( !( value is string ) ) {
				return new ValidationResult( false, "Value must be a string." );
			}
			if( this._exp == null ) {
				throw new ArgumentNullException( "Test expression is null." );
			}
			if( this._exp.IsMatch( (string)value ) ) {
				return new ValidationResult( true, null );
			}

			return new ValidationResult( false, this.ErrorMessage( "Input does not match required expression." ) );

		}

	} // class

} // namespace