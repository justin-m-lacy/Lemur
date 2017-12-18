using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Wraps a Predicate in a DependencyObject to allow for bindings.
	/// </summary>
	public class DependencyPredicate<T> : DependencyObject {

		public static readonly DependencyProperty PredicateProperty = DependencyProperty.Register( "Predicate",
typeof( Predicate<T> ), typeof( DependencyPredicate<T> ), new PropertyMetadata( null ) );

		/// <summary>
		/// Delegate for testing whether the new item is considered a match.
		/// </summary>
		public Predicate<T> Predicate {
			get { return (Predicate<T>)this.GetValue( PredicateProperty ); }
			set { this.SetValue( PredicateProperty, value ); }
		}

	} // class

} // namespace