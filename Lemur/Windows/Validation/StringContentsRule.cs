using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Rule supports invalid characters / required characters for strings. More complicated patterns should use the <see cref="RegexRule"/> class.
	/// </summary>
	public class StringContentsRule : CustomValidationRule {

		private char[] _forbiddenChars;
		public char[] Forbidden {
			get { return this._forbiddenChars; }
			set {
				this._forbiddenChars = value;
			}
		} //

		private char[] _requiredChars;
		public char[] Required {
			get { return this._requiredChars; }
			set {
				this._requiredChars = value;
			}
		}

		public StringContentsRule() {
		}

		public StringContentsRule( char[] required = null, char[] forbidden = null ) {

			this._requiredChars = required;
			this._forbiddenChars = forbidden;

		}

		public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {

			if( !( value is string ) ) {
				return new ValidationResult( false, "Value must be a string." );
			}

			string src = (string)value;

			if( this._forbiddenChars != null ) {

				char found;	/// forbidden character found, if any.
				if( this.contains( src, this._forbiddenChars, out found ) ) {
					return new ValidationResult( false, "Character: " + found + " not allowed." );
				}
			}
			if( this._requiredChars != null &&
				!contains( src, this._requiredChars) ) {
				return new ValidationResult( false, "Missing required characters." );
			}

			return new ValidationResult( true, null );

		} //

		/// <summary>
		/// Tests whether the input string contains any of the given
		/// test characters.
		/// 
		/// </summary>
		/// <param name="src"></param>
		/// <param name="test"></param>
		/// <returns></returns>
		private bool contains( string src, char[] test ) {

			int strlen = src.Length;
			int testlen = test.Length;

			for( int i = 0; i < strlen; i++ ) {

				char c = src[i];
				for( int j = 0; j < testlen; j++ ) {

					if( c == test[j] ) {
						return true;
					}

				} //

			} //
			return false;

		}

		/// <summary>
		/// Returns true if a character from the test array is found in the source string.
		/// The first test character found is returned in the output char parameter.
		/// If no test character is found, the output char is set to (char)0.
		/// </summary>
		/// <param name="src"></param>
		/// <param name="test"></param>
		/// <param name="found"></param>
		/// <returns></returns>
		private bool contains( string src, char[] test, out char found ) {

			int strlen = src.Length;
			int testlen = test.Length;

			for( int i = 0; i < strlen; i++ ) {

				char c = src[i];
				for( int j = 0; j < testlen; j++ ) {

					if( c == test[j] ) {

						found = c;
						return true;

					}

				} //

			} //

			found = (char)0;
			return false;

		} //

	} // class

} // namespace