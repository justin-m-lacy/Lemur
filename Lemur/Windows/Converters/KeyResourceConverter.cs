using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	/// <summary>
	/// Converts from a string key to a resource.
	/// </summary>
	public class KeyResourceConverter : DependencyObject, IValueConverter {

		private static readonly DependencyProperty ResourcesProperty = DependencyProperty.Register(
						"Resources",
						typeof( Dictionary<string, object> ),
						typeof( KeyResourceConverter ), new PropertyMetadata( new Dictionary<string,object>() ) );


		public Dictionary<string, object> Resources {
			get { return (Dictionary<string, object>)GetValue( ResourcesProperty ); }
			set {
				if( !Equals( value,null ) ) {
					this.SetValue( ResourcesProperty, value );
				}
			}
		}

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			Dictionary<string, object> resources = Resources;

			string key = value as string;
			if( !string.IsNullOrEmpty( key ) ) {

				object resource;
				if( resources.TryGetValue( key, out resource ) ) {
					return resource;
				}
				

			}

			//Console.WriteLine( "RESOURCE NOT found." );
			return Activator.CreateInstance( targetType );

		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
			throw new NotSupportedException();
		}

	} // class

} // namespace
