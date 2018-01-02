using Lemur.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// 
	/// Adapted from Brian Lagunas @ brianlagunas.com
	/// 
	/// </summary>
	public class EnumNameDescConverter : EnumConverter {

		public EnumNameDescConverter( Type type ) : base( type ) { }

		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType ) {

			if( destinationType == typeof( string ) ) {

				if( value != null ) {
	
					FieldInfo fi = value.GetType().GetField( value.ToString() );

					if( fi != null ) {

						var attributes = (NameDescAttribute[])fi.GetCustomAttributes( typeof( NameDescAttribute ), false );
						return ( ( attributes.Length > 0 ) && ( !String.IsNullOrEmpty( attributes[0].Name ) ) ) ? attributes[0].Name : value.ToString();

					}

				}

				return string.Empty;
			}

			// non-string destination.
			return base.ConvertTo( context, culture, value, destinationType );
		}

	} // class

} // namespace
