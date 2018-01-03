using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lemur.Utils {

	public static class DataUtils {

		/// <summary>
		/// Attempts to perform a deep-clone of the given source object.
		/// The source type must declare a public parameterless constructor.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <returns></returns>
		static public T DeepClone<T>( T src ) {

			T dest = Activator.CreateInstance<T>();

			CopyFields( src, dest );
			CopyProperties( src, dest );

			return dest;

		}

		static public void CopyProperties<T>( T src, T dest ) {

			Type ttype = typeof( T );

			PropertyInfo[] props = ttype.GetProperties();
			for( int i = props.Length - 1; i >= 0; i-- ) {

				PropertyInfo pinfo = props[i];
				if( !pinfo.CanWrite || !pinfo.CanRead ) {
					// must be able to read and write the property in order to copy it.
					continue;
				}

				object pvalue = pinfo.GetValue( src );
				Type propType = pinfo.PropertyType;

				if( !propType.IsValueType ) {
					// deep copy reference-type.
					pvalue = DeepClone( pvalue );
				}

				pinfo.SetValue( dest, pvalue );

			} // for-loop.

		}

		static public void CopyFields<T>( T src, T dest ) {

			Type ttype = typeof( T );

			FieldInfo[] fields = ttype.GetFields();
			for( int i = fields.Length - 1; i >= 0; i-- ) {

				FieldInfo finfo = fields[i];
				if( !finfo.Attributes.HasFlag( FieldAttributes.Public ) ) {
					continue;
				}

				object fvalue = finfo.GetValue( src );
				Type fieldType = finfo.FieldType;

				if( !fieldType.IsValueType ) {
					// deep copy reference-type.
					fvalue = DeepClone( fvalue );
				}

				finfo.SetValue( dest, fvalue );

			} // for-loop.

		}

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
