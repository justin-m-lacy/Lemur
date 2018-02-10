using Lemur.Windows.Text;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

		public static readonly DependencyProperty FeedbackProperty =
			DependencyProperty.Register( "Feedback", typeof( ICollection<TextString> ), typeof( FeedbackList ),
				new PropertyMetadata( null, FeedbackChanged ) );

		private static void FeedbackChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			FeedbackList fb = d as FeedbackList;

			INotifyCollectionChanged changed = e.OldValue as INotifyCollectionChanged;
			if( changed != null ) {
				changed.CollectionChanged -= fb.StringsChanged;
			}

			ICollection<TextString> textItems = e.NewValue as ICollection<TextString>;
			if( textItems != null ) {

				changed = textItems as INotifyCollectionChanged;
				if( changed != null ) {
					changed.CollectionChanged += fb.StringsChanged;
				}
				foreach( TextString s in textItems ) {
					fb.AddText( s );
				}

			}

		} // FeedbackChanged()

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
				( d as FeedbackList ).ClearList();
			}
			FeedbackList fb = d as FeedbackList;
			fb.AddText( msg_pair );

			// need to reset the value to allow multiple identical messages.
			d.SetCurrentValue( CurrentProperty, CurrentProperty.DefaultMetadata.DefaultValue );

		}

		public ICollection<TextString> Feedback {
			get => (ICollection<TextString>)GetValue( FeedbackProperty );
			set => SetValue( FeedbackProperty, value );
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

		private void StringsChanged( object sender, NotifyCollectionChangedEventArgs args ) {

			if( args.Action == NotifyCollectionChangedAction.Reset ) {
				this.Items.Clear();
				return;
			}

			if( args.OldItems != null ) {

				foreach( TextString s in args.OldItems ) {
					this.RemoveText( s );
				}

			}

			if( args.NewItems != null ) {

				foreach( TextString s in args.NewItems ) {
					this.AddText( s );
				}

			}



		} //

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

		public void AddText( TextString text ) {

			string msgType = text.Type;
			if( string.IsNullOrEmpty( msgType ) || msgType.ToLower() == TextString.Message ) {
				this.AddMessage( text.Text );
			} else {
				this.AddError( text.Text );
			}

		}

		public void RemoveText( TextString text ) {

			TextBlock b;
			for( int i = this.Items.Count - 1; i >= 0; i-- ) {

				b = this.Items[i] as TextBlock;
				if( b != null && b.Text == text ) {
					this.Items.RemoveAt( i );
					return;
				}

			}

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