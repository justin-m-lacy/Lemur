using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Utils {

	static class ArrayExtensions {

		static public string Join( this string[] strings, char separator=';' ) {

			if ( strings == null ) {
				return null;
			}

			int len = strings.Length;
			if ( len == 0 ) {
				return "";
			}

			string res = strings[0];
			for ( int i = 1; i < len; i++ ) {
				res += separator + strings[i];
			}

			return res;

		} //

	}

	static class StringExtensions {

		/// <summary>
		/// Returns the index of the first non-whitespace character.
		/// </summary>
		/// <param name="s"></param>
		/// <returns>Index of first nonwhite character, or -1 if string is null, empty,
		/// or contains all whitespace.</returns>
		static public int NonwhiteIndex( string s ) {

			if ( string.IsNullOrEmpty( s ) ) {
				return -1;
			}

			int len = s.Length;
			char c;

			/// end index of whitespace values.
			int endDigitIndex = 0;

			while ( endDigitIndex < len ) {

				c = s[endDigitIndex];
				if ( !char.IsWhiteSpace( c ) ) {
					return endDigitIndex;
				}
				endDigitIndex++;

			} // while

			return -1;

		} // NonwhiteIndex()

		static public int SortLenAscending( string a, string b ) {

			return -1;

		}

		static public bool Contains( this string s, char[] chars ) {

			for( int i = chars.Length - 1; i >= 0; i-- ) {
				if( s.Contains( chars[i] ) ) {
					return true;
				}
			}

			return false;

		} //

		static public string RemoveChars( this string s, char[] chars ) {

			int len = s.Length;
			StringBuilder b = new StringBuilder( s.Length );

			for ( int i = 0; i < len; i++ ) {

				char c = s[i];
				if ( !chars.Contains( c ) ) {
					b.Append( c );
				}

			}

			return b.ToString();

		}

	} // class

} // namespace
