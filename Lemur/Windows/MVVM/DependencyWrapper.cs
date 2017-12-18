using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Wraps an object into a dependency property so its value can be bound in xaml.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DependencyWrapper<T> : DependencyObject {

		public static DependencyProperty ValueProperty = DependencyProperty.Register( "Value",
			typeof( T ), typeof( DependencyWrapper<T> ) );

		private T value;
		public T Value {
			get => (T)this.GetValue( ValueProperty );
			set { this.SetValue( ValueProperty, value ); }
		}

		public DependencyWrapper() { }

		public DependencyWrapper( T startValue ) {
			this.value = startValue;
		}

		public static implicit operator DependencyWrapper<T>( T rhs ) {
			return new DependencyWrapper<T>( rhs );
		}

		public static implicit operator T( DependencyWrapper<T> rhs ) {
			return rhs.value;
		}

    } // class

} // namespace