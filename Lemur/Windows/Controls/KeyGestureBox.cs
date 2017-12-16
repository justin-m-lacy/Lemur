using Lemur.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lemur.Windows.Controls {

	/// <summary>
	/// Used to define a FreeKeyGesture.
	/// FreeKeyGesture allows key bindings without modifier keys.
	/// </summary>
	public class KeyGestureBox : TextBox {

		/*public static readonly RoutedEvent KeyGestureChangedEvent =
	EventManager.RegisterRoutedEvent( "KeyGestureChanged", RoutingStrategy.Bubble, typeof( RoutedEventHandler ),
		typeof( KeyGestureBox ) );

public event RoutedEventHandler KeyGestureChanged {
	add { AddHandler( KeyGestureChangedEvent, value ); }
	remove { RemoveHandler( KeyGestureChangedEvent, value ); }
}

void RaiseGestureChanged() {

	RoutedEventArgs args = new RoutedEventArgs( KeyGestureChangedEvent, this );
	this.RaiseEvent( args );

}*/

		
		/// <summary>
		/// Prompt to display in the textbox when user clicks the box to enter a new key.
		/// </summary>
		public static readonly DependencyProperty KeyPromptProperty
			= DependencyProperty.Register( "KeyPrompt", typeof( string ), typeof( KeyGestureBox ),
				new FrameworkPropertyMetadata {
					DefaultValue = "<Enter Key>",
					SubPropertiesDoNotAffectRender = true
				} );

		/// <summary>
		/// Prompt to display when a key should be entered.
		/// </summary>
		public string KeyPrompt {
			get { return (string)GetValue( KeyPromptProperty ); }
			set {
				SetValue( KeyPromptProperty, value );
			}
		}

		public static readonly DependencyProperty KeyGestureProperty = DependencyProperty.Register( "KeyGesture",
			typeof( FreeKeyGesture ), typeof( KeyGestureBox ),
			new FrameworkPropertyMetadata( new FreeKeyGesture(Key.None),
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
				new PropertyChangedCallback( KeyGestureUpdated )
			){
				DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged
			} );

		private static void KeyGestureUpdated( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			//Console.WriteLine( "KEY GESTURE UPDATED CALLBACK" );
			if( e.NewValue != null ) {
				( (KeyGestureBox)d ).Text = e.NewValue.ToString();
			} else {
				( (KeyGestureBox)d ).Text = Key.None.ToString();
			}

		}

		/// <summary>
		/// KeyGesture displayed in the KeyGestureBox.
		/// </summary>
		public FreeKeyGesture KeyGesture {
			get { return (FreeKeyGesture)GetValue( KeyGestureProperty ); }
			set {
				SetValue( KeyGestureProperty, value );
			} // set()
		}

		/// <summary>
		/// Key pressed for the last valid input.
		/// </summary>
		//private Key lastKey;

		/// <summary>
		/// Modifiers on the last valid input.
		/// </summary>
		//private ModifierKeys lastModifiers;

		public KeyGestureBox() : base() {

			//this.lastModifiers = ModifierKeys.None;
			//this.lastKey = Key.None;
			this.CaretBrush = null;
			this.IsReadOnlyCaretVisible = false;
			this.IsReadOnly = true;

		} //

		/*protected override void OnTextChanged( TextChangedEventArgs e ) {
			base.OnTextChanged( e );
			this.SelectAll();
		}*/

		protected override void OnPreviewMouseLeftButtonDown( MouseButtonEventArgs e ) {

			if( !this.IsKeyboardFocusWithin ) {
				this.Focus();
				this.SelectAll();
				e.Handled = true;
			}

		}

		protected override void OnGotKeyboardFocus( KeyboardFocusChangedEventArgs e ) {

			base.OnGotKeyboardFocus( e );

			this.Text = this.KeyPrompt;

			Console.WriteLine( "KEY FOCUS" );
			this.SelectAll();

		}

		protected override void OnLostKeyboardFocus( KeyboardFocusChangedEventArgs e ) {

			base.OnLostKeyboardFocus( e );
			if( this.KeyGesture != null ) {
				this.Text = this.KeyGesture.ToString();
			} else {
				this.Text = Key.None.ToString();
			}

		}

		protected override void OnKeyDown( KeyEventArgs e ) {

			Key mainKey = e.Key;
			ModifierKeys modifiers = e.KeyboardDevice.Modifiers;

			FreeKeyGesture gesture = new FreeKeyGesture( mainKey, modifiers );

			// not sure if this works with binding...
			this.KeyGesture = gesture;

			/// Don't enter text in the underlying TextBox.
			e.Handled = true;

		}

	} // class

} // namespace
