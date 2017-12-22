using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Lemur.Utils {

	static public class TypeUtils {

		/// <summary>
		/// Returns whether the given type contains a constructor which takes
		/// a single argument of the same Type, and is not a generic type definition.
		///
		/// NOTE: This does not prove the constructor found is intended for creating
		/// clones. This method should only be used when convention dictates the
		/// Type implement a Clone constructor as a possible cloning option.
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public static bool CanCloneByConstructor( Type t ) {

			TypeInfo info = t.GetTypeInfo();

			if( t.IsGenericType || t.ContainsGenericParameters ) {
				return false;
			}

			foreach( ConstructorInfo constructor in info.GetConstructors() ) {
			
				if( !constructor.IsPublic ) {
					continue;
				}
				ParameterInfo[] args = constructor.GetParameters();
				if( args.Length != 1 ) {
					continue;
				}
				ParameterInfo pInfo = args[0];
				if( pInfo.ParameterType.IsAssignableFrom( t ) ) {
					// Possible constructor found.
					return true;
				}

			} // for

			return false;

		} // CanCloneConstructor()

    } // class

} // namespace
