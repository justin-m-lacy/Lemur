using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lemur.Utils;
using static Lemur.Debug.DebugUtils;
using Lemur.Types;

namespace Lemur.Windows.Controls {

	/// <summary>
	/// TextBox for displaying memory units. (bytes, kb, mb, etc)
	/// </summary>
	public class ByteTextBox : TextBox {

		#region DEPENDENCY PROPERTIES
	
		public static readonly DependencyProperty AllowNegativeProperty = DependencyProperty.Register( "AllowNegative",
			typeof( bool ), typeof( ByteTextBox ), new PropertyMetadata( false ) );

		/// <summary>
		/// Whether to allow negative data sizes in the textBox.
		/// default is false
		/// </summary>
		public bool AllowNegative {
			get { return (bool)GetValue( AllowNegativeProperty ); }
			set { SetValue( AllowNegativeProperty, value ); }
		}

		public static readonly DependencyProperty MaxDecimalsProperty = DependencyProperty.Register( "MaxDecimals",
		typeof( int ), typeof( ByteTextBox ), new PropertyMetadata( 2 ) );

		/// <summary>
		/// Maximum number of decimal places to display in the TextBox.
		/// </summary>
		public int MaxDecimals {
			get { return (int)GetValue( MaxDecimalsProperty ); }
			set { SetValue( MaxDecimalsProperty, value ); }
		}

		#endregion

		/// <summary>
		/// TextBox reverts to this size on invalid inputs.
		/// </summary>
		private DataSize _lastValidSize;

		/// <summary>
		/// Attempts to convert the contents of the TextBox to an integer
		/// and returns the value.
		/// If there are any errors, 0 is returned.
		/// </summary>
		/// <returns></returns>
		public long GetBytes() {

			long value;
			if ( long.TryParse( this.Text, out value ) ) {
				return value;
			}
			return 0;

		}

		protected override void OnInitialized( EventArgs e ) {

			base.OnInitialized( e );

			this._lastValidSize = 0;

		}

		/// <summary>
		/// NOTE: Overriding previewKeyDown removes copy/paste automation.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown( KeyEventArgs e ) {

			base.OnKeyDown( e );

		}

		/// <summary>
		/// NOTE: this.Text is already changed at this point.
		/// </summary>
		/// <param name="e"></param>
		/*protected override void OnTextChanged( TextChangedEventArgs e ) {

			base.OnTextChanged( e );

		} // OnTextChanged()*/

		protected override void OnLostKeyboardFocus( KeyboardFocusChangedEventArgs e ) {

			base.OnLostKeyboardFocus( e );

			long newBytes;
			if ( DataSize.TryParse( this.Text, out newBytes ) ) {

				//Log( "NEW BYTES: " + newBytes );
				this._lastValidSize = new DataSize( newBytes );
				//Log( "RAW BYTES: " + this._lastValidSize );

				this.Text = this._lastValidSize.ToString();

			} else {

				Log( "invalid input: " + this.Text );
				this.Text = this._lastValidSize.ToString();

			}

		} //

		// NOTE: dependency properties not required.
		/**
		 * public static readonly DependencyProperty AllowNegativePropety =
			DependencyProperty.Register( "AllowNegative", typeof( bool ), typeof( ByteTextBox ),
				new FrameworkPropertyMetadata( false ) );

		/// <summary>
		/// Default units accessor.
		/// </summary>
		public bool AllowNegative {
			get { return (bool)GetValue( AllowNegativePropety ); }
			set { SetValue( AllowNegativePropety, value ); }

		}

		public static readonly DependencyProperty DefaultUnitsProperty =
			DependencyProperty.Register( "DefaultUnits", typeof( string ), typeof( ByteTextBox ),
				new FrameworkPropertyMetadata( string.Empty ) );

		/// <summary>
		/// Default units accessor.
		/// </summary>
		public string DefaultUnits {
			get { return (string)GetValue( DefaultUnitsProperty ); }
			set { SetValue( DefaultUnitsProperty, value ); }

		}**/

	} // class

} // namespace
