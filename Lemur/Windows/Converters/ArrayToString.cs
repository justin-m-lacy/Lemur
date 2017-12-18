using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Lemur.Windows.Converters {

	public class ArrayToString<T> : IValueConverter {

		private const char DefaultSeparator = ',';
		public string ErrorMessage = "Value must be an array.";

		#region PROPERTIES

		private char[] _separators;
		/// <summary>
		/// Separators for separating the string elements.
		/// </summary>
		public char[] Separators {
			get {
				return this._separators;
			}
			set {
				this._separators = value;
			}
		}

		/// <summary>
		/// Convenience accessor to avoid setting an entire array.
		/// </summary>
		public char Separator {

			get {
				return ( this._separators != null && this._separators.Length > 0 ) ? this._separators[0] :
					DefaultSeparator;
			}
			set {

				if( this._separators == null ) {
					this._separators = new[] { value };
				} else {

					/// append new value.
					int len = this._separators.Length;
					char[] new_arr = new char[ len ];
					this._separators.CopyTo( new_arr, 0 );
					new_arr[len - 1] = value;
				}

			}
		}

		#endregion

		/// <summary>
		/// Returns the empty string on null input.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value == null ) {
				return string.Empty;
			}
			Type valType = value.GetType();
			if( !valType.IsArray ) {
				throw new ArgumentException( ErrorMessage );
			}

			Type expectedType = typeof( T );
			// The array elements of the input object should be assignable to array elements of the expected type.
			if( !expectedType.IsAssignableFrom( valType.GetElementType() ) ) {
				throw new ArgumentException( "Array is not of type: " + expectedType.FullName );
			}

			return this.BuildArrayString( (Array)value );

		}

		private string BuildArrayString( Array a ) {

			if( a.Length == 0 ) {
				return string.Empty;
			}

			char separator;
			if( _separators == null || this._separators.Length == 0 ) {
				separator = DefaultSeparator;
			} else {
				separator = this._separators[0];
			}

			StringBuilder output = new StringBuilder();

			foreach( T elm in a ) {

				output.Append( elm.ToString() );
				output.Append( separator );

			} //

			output.Remove( output.Length - 1, 1 );

			return output.ToString();

		} //

		/// <summary>
		/// Conversion from string to T[]
		/// If value is null or a string of length 0, an empty array is returned.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value == null ) {
				return new T[0];
			}
			if( !( value is string ) ) {
				throw new ArgumentException( "Value must be a string." );
			}


			string res = (string)value;
			string[] subs;
			if( this._separators == null ) {
				subs = res.Split( DefaultSeparator );
			} else {
				subs = res.Split( this._separators );
			}


			int len = subs.Length;
			if( len == 0 ) {
				return new T[0];
			}

			T[] array = new T[len];
			for( int i = 0; i < len; i++ ) {

				array[i] = (T)System.Convert.ChangeType( subs[i], targetType );

			}

			return array;

		} // ConvertBack()

	} // ArrayToString<T> class

	/// <summary>
	/// Convenience optimized class StringArrayToString.
	/// </summary>
	public class StringArrayToString : IValueConverter {

		#region PROPERTIES

		private const char DefaultSeparator = ',';
		public string ErrorMessage = "Value must be an array.";

		private char[] _separators;
		/// <summary>
		/// Separators for separating the string elements.
		/// </summary>
		public char[] Separators {
			get {
				return this._separators;
			}
			set {
				this._separators = value;
			}
		}

		/// <summary>
		/// Convenience accessor to avoid setting an entire array.
		/// </summary>
		public char Separator {

			get {
				return ( this._separators != null && this._separators.Length > 0 ) ? this._separators[0] :
					DefaultSeparator;
			}
			set {

				if( this._separators == null ) {
					this._separators = new[] { value };
				} else {

					/// append new value.
					int len = this._separators.Length;
					char[] new_arr = new char[len];
					this._separators.CopyTo( new_arr, 0 );
					new_arr[len - 1] = value;
				}

			}
		}

		#endregion

		/// <summary>
		/// Returns the empty string on null input.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value == null ) {
				return string.Empty;
			}
			if( !( value is string[] ) ) {
				throw new ArgumentException( "Value must be a string array." );
			}

			return this.BuildArrayString( (string[])value );

		}

		private string BuildArrayString( string[] a ) {

			int len = a.Length;
			if( len == 0 ) {
				return string.Empty;
			} else if( len == 1 ) {
				return a[0].ToString();
			}

			char separator;
			if( _separators == null || this._separators.Length == 0 ) {
				separator = DefaultSeparator;
			} else {
				separator = this._separators[0];
			}

			StringBuilder output = new StringBuilder();
			output.Append( a[0] );

			for( int i = 1; i < len; i++ ) {

				output.Append( separator );
				output.Append( a[i] );

			} //

			return output.ToString();

		} //

		/// <summary>
		/// Conversion from string to string[]
		/// If value is null or a string of length 0, an empty array is returned.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {

			if( value == null ) {
				return new string[0];
			}
			if( !( value is string ) ) {
				throw new ArgumentException( "Value must be a string." );
			}

			string res = (string)value;
			string[] subs;
			if( this._separators == null ) {
				subs = res.Split( DefaultSeparator );
			} else {
				subs = res.Split( this._separators );
			}

			return subs;

		} // ConvertBack()

	} // ArrayToString<T> class

} // namespace
