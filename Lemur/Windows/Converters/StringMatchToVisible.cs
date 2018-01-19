using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Returns Visiblity.Visible if the bound string matches the TargetString, and Hidden otherwise.
	/// </summary>
	public class StringMatchToVisible : DependencyObject, IValueConverter {

		public static readonly DependencyProperty TargetStringProperty = DependencyProperty.Register( "TargetString", typeof( string ),
			typeof( StringMatchToVisible ) );

		/// <summary>
		/// Whether to Invert the match behaviour:
		/// i.e. Hide matches and Reveal non-matches.
		/// </summary>
		public bool Invert {
			get;
			set;
		}

		/// <summary>
		/// Behaviour to use when hiding the matched element.
		/// </summary>
		private Visibility _hideBehavior = Visibility.Collapsed;
		public Visibility HideBehavior {
			get { return this._hideBehavior; }
			set { this._hideBehavior = value; }
		}

		/// <summary>
		/// String to match in order for the Converter to return Visible.
		/// </summary>
		public string TargetString {
			get { return (string)this.GetValue( TargetStringProperty ); }
			set {
				
				if( value != this.TargetString ) {
					this.SetValue( TargetStringProperty, value );
				}
			}

		}

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( Object.Equals( value, null ) || !( value is string ) ) {
				return this.Invert ? Visibility.Visible : this.HideBehavior;
			}

			string s = (string)value;
			if( s == TargetString ) {
				return Invert ? this.HideBehavior : Visibility.Visible;
			}

			return this.Invert ? Visibility.Visible : this.HideBehavior;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotSupportedException();
		}

	} // class

} // namespace
