using Lemur.Windows.MVVM;
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
	public class UniqueItemRule<T> : ValidationRule {

		private DependencyWrapper<ICollection<T>> _items;
		public DependencyWrapper<ICollection<T>> Items {
			get { return this._items; }
			set { this._items = value; }
		}

		public RuleProperties Properties { get; set; }

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			if( Items == null || Items.Value == null || Items.Value.Count == 0 ) {
				return new ValidationResult( true, null );
			}
	
			foreach( T obj in Items.Value ) {
				if( value.Equals( obj ) ) {
					return new ValidationResult( false, Properties.ErrorMessage );
				}
			}

			return new ValidationResult( false, Properties.ErrorMessage );

		}

	} // class

} // namespace
