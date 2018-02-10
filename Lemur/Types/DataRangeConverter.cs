using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Lemur.Types {

	public class DataRangeConverter : TypeConverter {

		private const char SEPARATOR_CHAR = ':';

		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType ) {

			if( sourceType == null ) {
				return true;
			}
			if( sourceType != typeof( string ) ) {
				return false;
			}
			return true;

		}

		public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value ) {

			if( value == null ) {
				return new DataRange();
			}

			if( !( value is string ) ) {
				throw new NotSupportedException();
			}

			string dataString = (string)value;
			if( string.IsNullOrEmpty( dataString ) ) {
				return new DataRange();
			}
			string[] parts = dataString.Split( SEPARATOR_CHAR );

			if( parts.Length == 0 ) {
				return new DataRange();
			}

			long max;

			if( parts.Length == 1 ) {

				if( long.TryParse( parts[0], out max ) ) {

					// only maxrange defined.
					return new DataRange( max );
				}
	
			}

			long min;
			if( long.TryParse( parts[0], out min ) && long.TryParse( parts[1], out max ) ) {
				// both minrange and minrange defined.
				return new DataRange( min, max );
			}

			return new DataRange();

		} //


		public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType ) {
			if( destinationType == null || destinationType != typeof( string ) ) {
				return false;
			}
			return true;
		}
	
		public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType ) {

			if( destinationType == null ) {
				throw new ArgumentNullException();
			}
			if( destinationType != typeof( string ) ) {
				throw new NotSupportedException();
			}
			DataRange range = (DataRange)value;

			return range.MinSize.Bytes.ToString() + SEPARATOR_CHAR + range.MaxSize.Bytes.ToString();

		} //

	} // class

} // namespace
