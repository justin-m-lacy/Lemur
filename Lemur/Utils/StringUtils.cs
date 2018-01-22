using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Utils {

	public static class StringUtils {

		/// <summary>
		/// Strip out any name characters that would not be allowed in a typical
		/// variable name.
		/// </summary>
		/// <param name="name"></param>
		public static string MakeVarName( string name ) {

			char[] chars = name.ToCharArray();
			int maxCount = chars.Length;

			StringBuilder b = new StringBuilder( maxCount );
			for( int i = 0; i < maxCount; i++ ) {

				char c = chars[i];
				if( char.IsLetterOrDigit( c ) ) {
					b.Append( c );
				}

			}

			return b.ToString();

		}

    } // class

} // namespace
