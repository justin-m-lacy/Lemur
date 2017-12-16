using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Utils {

	static public class Extensions {

		/// <summary>
		/// Adapted from StackOverflow.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		static bool IsNumber( this object obj ) {

			/// this check was in original but if obj is null, how is the function
			/// being called on object?!?! Need to test...
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

		} //

    } // class

} // namespace
