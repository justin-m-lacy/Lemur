using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;

namespace Lemur.Windows.Controls {

	class IntegerBox : TextBox {

		private string _lastValidText;

		/// <summary>
		/// Attempts to convert the contents of the TextBox to an integer
		/// and returns the value.
		/// If there are any errors, 0 is returned.
		/// </summary>
		/// <returns></returns>
		public int GetIntValue() {

			int value;
			if ( int.TryParse( this.Text, out value ) ) {
				return value;
			}
			return 0;

		}

		protected override void OnInitialized( EventArgs e ) {

			base.OnInitialized( e );
			this._lastValidText = this.Text;

		}

		/// <summary>
		/// NOTE: Overriding previewKeyDown removes copy/paste automation.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown( KeyEventArgs e ) {

			base.OnKeyDown( e );

			char theChar = (char)KeyInterop.VirtualKeyFromKey( e.Key );

			if ( !Char.IsDigit( theChar ) ) {
				e.Handled = true;
			}

		}

		/// <summary>
		/// NOTE: this.Text is already changed at this point.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged( TextChangedEventArgs e ) {

			base.OnTextChanged( e );

			int value;
			if ( !int.TryParse( this.Text, out value ) ) {

				this.Text = this._lastValidText;
				e.Handled = true;
			}

		}

	} // class

} // namespace