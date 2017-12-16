using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Boolean to Visible converter with extra options.
	/// </summary>
	public class BoolToVisible : IValueConverter {

		/// <summary>
		/// Whether to invert the boolean before applying the visibility.
		/// </summary>
		public bool Invert {
			get;
			set;
		}

		/// <summary>
		/// Mode to use when the element isn't visible.
		/// </summary>
		public Visibility hideMode = Visibility.Collapsed;
		public Visibility HideMode {
			get { return this.hideMode; }
			set {
				if( hideMode != value ) {
					hideMode = value;
				}
			}
		}

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( Equals( value, null ) || !(value is bool) ) {
				return this.Invert ? Visibility.Visible : hideMode;
			}
			bool visible = this.Invert ? !(bool)value : (bool)value;

			return visible ? Visibility.Visible : this.hideMode;

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotImplementedException();
		}

	} // class

} // namespace
