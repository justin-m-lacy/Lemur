using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Validates data according to a Predicate validation rule.
	/// If the type of the data is incorrect, the data is automatically rejected.
	/// If the type is correct but no predicate is specified, the data is automatically accepted.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PredicateRule<T> : ValidationRule {

		private DependencyPredicate<T> _rule;
		public DependencyPredicate<T> Rule {
			get { return this._rule; }
			set => this._rule = value;
		}

		private RuleProperties properties;
		public RuleProperties Properties { get => properties; set => properties = value; }

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			if( !( value is T ) ) {
				// Using full namespace to avoid 'ValidationResult' name collisions.
				throw new System.ComponentModel.DataAnnotations.ValidationException( "Unexpected type." );
			}

			if( Rule == null || Rule.Predicate == null ) {
				return new ValidationResult( true, null );
			}

			Predicate<T> validTest = Rule.Predicate;

			T item = (T)value;
			if( validTest( item ) ) {
				return new ValidationResult( false, Properties.ErrorMessage );
			}

			return new ValidationResult( false, Properties.ErrorMessage );

		}

	} // class

} // namespace
