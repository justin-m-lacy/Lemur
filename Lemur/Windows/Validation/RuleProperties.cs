using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Useful properties for binding ValidationRules.
	/// </summary>
	public class RuleProperties<T> : DependencyObject {

		public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register( "DuplicateMessage",
				typeof( string ), typeof( RuleProperties<T> ), new PropertyMetadata( "Invalid entry." ) );

		public string ErrorMessage {
			get { return (string)this.GetValue( ErrorMessageProperty ); }
			set { this.SetValue( ErrorMessageProperty, value ); }
		}

		public static readonly DependencyProperty ValidationTestProperty = DependencyProperty.Register( "ValidationTest",
	typeof( Predicate<T> ), typeof( RuleProperties<T> ), new PropertyMetadata( null ) );

		/// <summary>
		/// Delegate for testing whether the new item is considered a match.
		/// </summary>
		public Predicate<T> ValidationTest {
			get { return (Predicate<T>)this.GetValue( ValidationTestProperty ); }
			set { this.SetValue( ValidationTestProperty, value ); }
		}

		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register( "Items",
typeof( IEnumerable<T> ), typeof( RuleProperties<T> ), new PropertyMetadata( null ) );

		/// <summary>
		/// Optional property. Existing items already being compared.
		/// </summary>
		public IEnumerable<T> Items {
			get { return (IEnumerable<T>)this.GetValue( ItemsProperty ); }
			set { this.SetValue( ItemsProperty, value ); }
		}

	} // class

} // namespace
