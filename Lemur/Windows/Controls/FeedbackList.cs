using Lemur.Windows.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lemur.Windows.Controls {

	/// <summary>
	/// Display a list of feedback items of different colors.
	/// </summary>
	public class FeedbackList : ItemsControl {

		static FeedbackList() {
			DefaultStyleKeyProperty.OverrideMetadata( typeof( FeedbackList ), new FrameworkPropertyMetadata(typeof(FeedbackList)) );
		}

		public static readonly DependencyProperty ErrorBrushProperty =
			DependencyProperty.Register( "ErrorBrush", typeof( Brush ), typeof( FeedbackList ),
				new PropertyMetadata( new SolidColorBrush( Color.FromRgb( 0x7b, 0, 0 ) ) ) );

		public static readonly DependencyProperty DefaultBrushProperty =
			DependencyProperty.Register( "DefaultBrush", typeof( Brush ), typeof( FeedbackList ),
				new PropertyMetadata( new SolidColorBrush( Color.FromRgb( 0, 0, 0 ) ) ) );

		public static readonly DependencyProperty ClearProperty =
			DependencyProperty.Register( "Clear", typeof( bool ), typeof( FeedbackList ), new PropertyMetadata( false, ClearChangedCallback ) );

		private static void ClearChangedCallback( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			if( (bool)e.NewValue == true ) {
				( d as FeedbackList ).ClearList();
				// Allow clear to be called again.
				d.SetCurrentValue( ClearProperty, false );
			}

		}

		/// <summary>
		/// next message being added.
		/// The tuple is a string message and a message type.
		/// If the message type is null or empty, the default message type is used.
		/// </summary>
		public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register( "Current",
			typeof( TextString ), typeof( FeedbackList ), new PropertyMetadata( new TextString(null), CurrentChangedCallback ) );

		private static void CurrentChangedCallback( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			TextString msg_pair = (TextString)e.NewValue;
			if( string.IsNullOrEmpty( msg_pair.Text ) ) {
				return;
			}
			FeedbackList fb = d as FeedbackList;

			string msgType = msg_pair.Type;
			if( string.IsNullOrEmpty( msgType ) || msgType.ToLower() == TextString.Message ) {
				fb.AddMessage( msg_pair.Text );
			} else {
				fb.AddError( msg_pair.Text );
			}

			// need to reset the value to allow multiple identical messages.
			d.SetCurrentValue( CurrentProperty, CurrentProperty.DefaultMetadata.DefaultValue );

		}

		public Brush ErrorBrush {
			get => (Brush)GetValue( ErrorBrushProperty );
			set => SetValue( ErrorBrushProperty, value );
		}

		public Brush DefaultBrush {
			get => (Brush)GetValue( DefaultBrushProperty );
			set => SetValue( DefaultBrushProperty, value );
		}

		/// <summary>
		/// Clears the list.
		/// </summary>
		public bool Clear {

			set {
				if( value == true ) {
					this.ClearList();
				}
			}

		}

		/// <summary>
		/// Set the current message. By default, the new message is added
		/// to the end of the list.
		/// </summary>
		public TextString Current {
			get => (TextString)GetValue( CurrentProperty );
			set => this.SetValue( CurrentProperty, value );

		}

		public FeedbackList() {
		}

		public void ClearList() {
			this.Items.Clear();
		}

		public void AddError( string msg ) {

			TextBlock box = new TextBlock();
			box.Foreground = this.ErrorBrush;
			box.Text = msg;

			this.Items.Add( box );

		}

		public void AddMessage( string msg ) {

			TextBlock box = new TextBlock();
			box.Foreground = this.DefaultBrush;
			box.Text = msg;

			this.Items.Add( box );

		}

	} // class

} // namespace