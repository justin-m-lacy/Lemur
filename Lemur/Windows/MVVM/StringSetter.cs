using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Sets a string value on a bound object.
	/// NOTE: This class works but feels heavy for what it accomplishes.
	/// It has to be a FrameworkElement just to have access to the DataContext binding.
	/// </summary>
	public class StringSetter : FrameworkElement {

		/// <summary>
		/// Binding should be connected to the property to set in the DataContext.
		/// </summary>
		static public readonly DependencyProperty BindingProperty = DependencyProperty.Register( "Binding", typeof( string ), typeof( StringSetter ));

		/// <summary>
		/// The value to set on the binding.
		/// </summary>
		static public readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof( string ), typeof( StringSetter ),
			new PropertyMetadata( ValueChanged ) );

		/// <summary>
		/// The callback is necessary to get the property to update from xaml. (Can't set it in the Value-setter)
		/// This is because xaml appears to set the value in the backing-table directly without calling the property setter.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void ValueChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
			(d as StringSetter ).Binding = (string)e.NewValue;
		}

		public string Value {
			get { return (string)this.GetValue( ValueProperty ); }
			set {
				this.SetValue( ValueProperty, value );
			}

		}

		public string Binding {
			get => (string)this.GetValue( BindingProperty );
			set => this.SetValue( BindingProperty, value );
		}

		public StringSetter() {
		}

    } // class

} // namespace
