using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Validation rule that ensures the new value is unique to the existing
	/// list of items.
	/// </summary>
	public class UniqueRule<T> : ValidationRule {

		public RuleProperties<T> Properties { get; set; }

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			if( Properties == null || Properties.Items == null ) {
				return new ValidationResult( true, null );
			}

			Predicate<T> validTest = Properties.ValidationTest;

			if( validTest != null ) {

				if( !(value is T) ) {
					// Using full namespace to avoid 'ValidationResult' name collisions.
					throw new System.ComponentModel.DataAnnotations.ValidationException( "Unexpected type.");
				}

				T item = (T)value;
				if( validTest( item ) ) {
					return new ValidationResult( false, Properties.ErrorMessage );
				}
	

			} else {
	
				foreach( T obj in Properties.Items ) {
					if( value.Equals( obj ) ) {
						return new ValidationResult( false, Properties.ErrorMessage );
					}
				}
	
			}

			return new ValidationResult( false, Properties.ErrorMessage );

		}

	} // class

} // namespace
