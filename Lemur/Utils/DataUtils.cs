using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Utils {

	public static class DataUtils {

		static public bool IsNumber( object obj ) {

			if( Equals( obj, null ) ) {
				return false;
			}

			Type objType = obj.GetType();
			if( objType.IsPrimitive ) {

				return ( objType == typeof( int ) || objType == typeof( uint )
					|| objType == typeof( long ) || objType == typeof( ulong ) ||
					objType == typeof( byte ) || objType == typeof( double )
					|| objType == typeof( float ) );
			}

			return objType == typeof( decimal );

		}

    } // class

} // namespace
