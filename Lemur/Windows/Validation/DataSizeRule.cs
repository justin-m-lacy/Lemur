using Lemur.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Validation Rule that requires the Binding value must be a valid
	/// data size. (string or long representing a memory size.)
	/// </summary>
	public class DataSizeRule : CustomValidationRule {

		public DataSizeRule() {
			this.ValidationStep = ValidationStep.RawProposedValue;
			this.DefaultErrorMessage = "Input is not a valid data size.";
		}

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			if( value is string ) {

				if( DataSize.IsValidSize( (string)value ) ) {
					return new ValidationResult( true, null );
				}

			} else if( value is long || value is int || value is uint || value is ulong ) {

				return new ValidationResult( true, null );
			} else if( value is DataSize ) {
				return new ValidationResult( true, null );
			}

			return new ValidationResult( false, this.DefaultErrorMessage );

		} //

	} // class

} // namespace
