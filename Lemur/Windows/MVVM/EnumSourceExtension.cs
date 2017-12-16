using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Code based on class by Brian Lagunas @ brianlagunas.com
	/// </summary>
	public class EnumSourceExtension : MarkupExtension {

		/// <summary>
		/// Type of enum whose values serve as the binding source.
		/// </summary>
		private Type _enumType;
		public Type EnumType {
			get { return this._enumType; }
			set {

				if( value == this._enumType ) {
					return;
				}

				if( value != null ) {
					/// checks that the Type assigned really is an Enum.
					Type enumType = Nullable.GetUnderlyingType( value ) ?? value;
					if( !enumType.IsEnum )
						throw new ArgumentException( "Type must be an Enum" );

				}

				this._enumType = value;

			} // set()

		}

		public EnumSourceExtension() { }
		public EnumSourceExtension( Type enumType ) {
			this.EnumType = enumType;
		}
		
		public override object ProvideValue( IServiceProvider serviceProvider ) {

			Type enumType = this._enumType;
			if( enumType == null ) {
				return Array.CreateInstance( typeof(string), 0 );
			}

			// seems to be checking if enum is wrapped in a Nullable?
			Type actualType = Nullable.GetUnderlyingType( enumType ) ?? enumType;

			Array enumValues = Enum.GetValues( actualType );

			if( actualType == enumType ) {
				return enumValues;
			}

			// Not sure what the +1 is doing here. Maybe to leave a 'null' option?
			Array shiftArray = Array.CreateInstance( actualType, enumValues.Length + 1 );
			enumValues.CopyTo( shiftArray, 1 );

			return shiftArray;

		} //

	} // class

} // namespace
