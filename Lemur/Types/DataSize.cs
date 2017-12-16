using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using Lemur.Utils;

using static Lemur.Debug.DebugUtils;
using System.Diagnostics;

namespace Lemur.Types {

	/// <summary>
	/// Utilities for reading and displaying data sizes ( bytes, megabytes, gigabytes, etc. )
	/// </summary>
   public struct DataSize : IComparable<DataSize>, IEquatable<DataSize> {

		/// <summary>
		/// Multiplicative factor between unit bases. (kb,mb,gb,etc.)
		/// </summary>
		private const int UnitSizeFactor = 1024;

		/// <summary>
		/// Nubmer of left-bit shifts between each size base.
		/// </summary>
		private const int UnitShiftStep = 10;

		private const char ByteSymbol = 'b';
		private const string ByteWord = "bytes";

		/// <summary>
		/// 'byte' prefixes for units.
		/// strings are used to allow for an empty 'byte' prefix.
		/// </summary>
		static readonly string[] UnitPrefixes = { "", "k", "m", "g", "t", "p", "e", "z", "y" };


		static readonly string[] UnitNames = { "", "kilo", "mega", "giga", "tera", "peta", "exa", "zetta", "yotta" };

		/// <summary>
		/// Internal variable for computing unit strings.
		/// </summary>
		static private Regex _unitRegEx;
		static public Regex UnitRegEx {

			get {
				return _unitRegEx ?? ( _unitRegEx = new Regex(

					@"^\s*(?<unit>([" + CollectionUtils.Join( DataSize.UnitPrefixes ) + @"]|" + UnitNames.Join( '|' ) + @")?)"

						+ @"([" + DataSize.ByteSymbol + @"]|" + DataSize.ByteWord + @")?\s*$",
					RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture
				) );

			}

		}

		/// <summary>
		/// Internal variable for SizeRegEx.
		/// </summary>
		static private Regex _sizeRegEx;

		/// note that either a digit must be found before the decimal
		/// or a digit must be found after the decimal.
		/// NOTE: this was tested with: http://regexstorm.net/tester but allows strange
		/// inputs such as -.0, which is interpreted as plain 0.
		static public Regex SizeRegEx {

			get {

				return _sizeRegEx ?? ( _sizeRegEx = new Regex(
					@"^\s*" +
						@"(?<size>\-?(\d+\.?\d*|\d*\.?\d+))\s*" +
						@"(?<unit>([" + CollectionUtils.Join( DataSize.UnitPrefixes ) + @"]|" + UnitNames.Join('|') + @")?)"

						+ @"([" + DataSize.ByteSymbol + @"]|" + DataSize.ByteWord + @")?\s*$",
						RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Compiled )
					);

			} // get

		} //  SizeRegEx

		/// <summary>
		/// Useful for testing.
		/// </summary>
		/// <returns></returns>
		static public string GetRegExString() {
			return DataSize.SizeRegEx.ToString();
		}

		/// <summary>
		/// The size being represented, in bytes.
		/// </summary>
		private long bytes;
		/// <summary>
		/// The size being represented, in bytes.
		/// </summary>
		public long Bytes {
			get {
				return this.bytes;
			}
		}

		public DataSize( long byteSize ) {

			this.bytes = byteSize;

		} // DataSize()

		/// <summary>
		/// Create a new DataSize object with a memory size in a given unit base.
		/// </summary>
		/// <param name="size"></param>
		/// <param name="units"></param>
		public DataSize( long size, string units ) {

			int unitBase;
			if( DataSize.TryGetUnitBase( units, out unitBase ) ) {

				this.bytes = size * ( unitBase );

			} else {
				
				this.bytes = 0;
			}

		} // DataSize()


		/// <summary>
		/// Given a unit name or abbreviation, returns the number of bytes in the given
		/// unit base.
		/// Returns -1 if the units string is invalid.
		/// the units string is assumed to be a valid unit prefix string.
		/// </summary>
		/// <param name="unitPrefix"></param>
		/// <returns></returns>
		static private int GetUnitBase( string unitPrefix ) {

			if ( string.IsNullOrEmpty( unitPrefix ) ) {
				return 1;
			}

			int unitPower;

			// attempt to get a simple unit index.
			if( unitPrefix.Length <= 1 ) {
				unitPower = Array.IndexOf( DataSize.UnitPrefixes, unitPrefix );
			} else {

				unitPower = Array.IndexOf( DataSize.UnitNames, unitPrefix );
			}

			if( unitPower >= 0 ) {
				return 1 << ( unitPower * UnitShiftStep );
			}
			return unitPower;


		}

		/// <summary>
		/// Attempts to parse the unit string into a base number of bytes.
		/// </summary>
		/// <param name="units"></param>
		/// <param name="unitBase"></param>
		/// <returns></returns>
		static public bool TryGetUnitBase( string units, out int unitBase ) {

			if( string.IsNullOrEmpty( units ) ) {
				unitBase = 1;
				return true;
			}

			Match match = UnitRegEx.Match( units );
			if( !match.Success ) {
				unitBase = -1;
				return false;
			}

			string unitPrefix = match.Groups[0].Value;

			int unitPower;
			// attempt to get a simple unit index.
			if( unitPrefix.Length <= 1 ) {
				unitPower = Array.IndexOf( DataSize.UnitPrefixes, unitPrefix );
			} else {
				unitPower = Array.IndexOf( DataSize.UnitNames, unitPrefix );
			}

			if( unitPower >= 0 ) {
				unitBase = 1 << ( unitPower * UnitShiftStep );
			} else {
				unitBase = -1;
			}

			return ( unitBase >= 1 );

		}

		/// <summary>
		/// Returns the given number of bytes as a formatted string, using a unit base
		/// which keeps the result below the given maximum unit size.
		/// </summary>
		/// <param name="bytes">Number of bytes to display as a string.</param>
		/// <param name="decimalPlaces">Number of decimal places allowed in the result.</param>
		/// <param name="maxUnitSize">The bytes value will be reported in a unit base which keeps
		/// the converted value below this number.
		/// For example: If the bytes value is 2048, and the max unit size is 1024, the result
		/// will be reported as 2.00kb.
		/// However if the bytes value is 2048 and the max unit size is 4096, the result will be reported
		/// as 2048 bytes.</param>
		/// <returns></returns>
		static public string GetString( long bytes, uint decimalPlaces, uint maxUnitSize=DataSize.UnitSizeFactor ) {

			int basePower = 0;
			int maxPower = DataSize.UnitPrefixes.Length - 1;
			long nextUnitSize = maxUnitSize;
			long converted = bytes;

			while ( converted > nextUnitSize && basePower <= maxPower ) {

				nextUnitSize <<= DataSize.UnitShiftStep;
				basePower++;

			}

			/// nextUnitSize overshoots the actual units needed. baseUnits are the correct units at this point.
			double decimalVal = ( (double)converted ) / ( 1 << (basePower*UnitShiftStep) );

			return decimalVal.ToString( "F" + decimalPlaces ) + DataSize.UnitPrefixes[basePower] + DataSize.ByteSymbol;

		}

		/// <summary>
		/// Checks if a string represents a correctly formatted data size.
		/// </summary>
		/// <param name="sizeString"></param>
		/// <returns></returns>
		static public bool IsValidSize( string sizeString ) {

			if ( string.IsNullOrEmpty( sizeString ) ) {
				return false;
			}

			/// regular expression for evaluating data sizes.
			Regex exp = DataSize.SizeRegEx;

			Match match = exp.Match( sizeString );
			if ( !match.Success ) {
				return false;
			}

			return true;

		} // IsValidSize()

		/// <summary>
		/// Attempts to interpret the given string as a data size.
		/// </summary>
		/// <param name="sizeString"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		static public bool TryParse( string sizeString, out long bytes ) {

			if ( string.IsNullOrEmpty( sizeString ) ) {
				bytes = 0;
				return false;
			}

			/// regular expression for evaluating data sizes.
			Regex exp = DataSize.SizeRegEx;

			Match match = exp.Match( sizeString );
			if ( match.Success ) {

				GroupCollection groups = match.Groups;
				string numStr = groups["size"].Value;
				string unitStr = groups["unit"].Value;

				//Log( "num: " + numStr );
				//Log( "units: " + unitStr );

				double numValue;
				if ( double.TryParse( numStr, out numValue ) ) {
					return TryGetBytes( numValue, unitStr, out bytes );
				}
				
			}

			bytes = 0;
			return false;

		} //

		/// <summary>
		/// Attempts to interpret the given string as a data size.
		/// </summary>
		/// <param name="sizeString"></param>
		/// <param name="dataSize"></param>
		/// <returns></returns>
		static public bool TryParse( string sizeString, out DataSize dataSize ) {

			if( string.IsNullOrEmpty( sizeString ) ) {
				dataSize = new DataSize();
				return false;
			}

			/// regular expression for evaluating data sizes.
			Regex exp = DataSize.SizeRegEx;

			Match match = exp.Match( sizeString );
			if( match.Success ) {

				GroupCollection groups = match.Groups;
				string numStr = groups["size"].Value;
				string unitStr = groups["unit"].Value;

				Console.WriteLine( "unit: " + unitStr );

				double numValue;
				if( double.TryParse( numStr, out numValue ) ) {

					long bytes = 0;
					if( DataSize.TryGetBytes( numValue, unitStr, out bytes ) ) {
						dataSize = new DataSize( bytes );
						return true;
					}

				}

			}

			dataSize = new DataSize();
			return false;

		} //

		/// <summary>
		/// Converts a string size with optional unit postfix into
		/// a raw byte size.
		/// e.g. 1024kb into 1024000.
		/// </summary>
		/// <param name="sizeString"></param>
		/// <returns>The string value converted to bytes, or 0 on failure.</returns>
		static public long GetAsBytes( string sizeString ) {

			long bytes;
			if ( TryParse( sizeString, out bytes ) ) {
				return bytes;
			}
			return 0;

		} // GetBytes()

		/// <summary>
		/// Converts a decimal figure and a size-unit postfix into a raw size in bytes.
		/// Valid postfix strings include 'k','kb','m','mb','g','gb','t','tb', 'b',
		/// and any uppercase/lowercase variations of these.
		/// If an invalid string begins with a valid character, the valid character is used.
		/// Any other invalid postfix, including null and empty, is treated as kilobytes for the purposes
		/// of determining the size unit.
		/// </summary>
		/// <param name="decimalVal"></param>
		/// <param name="units">String representing the units of the data.</param>
		/// <returns></returns>
		static public long GetAsBytes( double decimalVal, string units ) {

			long result;
			if ( DataSize.TryGetBytes( decimalVal, units, out result ) ) {
				return result;
			}
			return 0;

		} //()

		/// <summary>
		/// </summary>
		/// <param name="decimalVal"></param>
		/// <param name="units"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		static public bool TryGetBytes( double decimalVal, string units, out long bytes ) {

			int unitBase = DataSize.GetUnitBase( units );

			Console.WriteLine( "unit base: " + unitBase );
			if ( unitBase >= 0 ) {

				bytes = (long)( decimalVal * ( unitBase) );
				return true;

			}

			bytes = 0;
			return false;

		}

		/// <summary>
		/// Returns a long byte size as a dataSize string.
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="fullUnitNames">Whether to display units as a full word, instead
		/// of an abbreviation.</param>
		/// <returns></returns>
		static public string ToDataString( long bytes, bool fullUnitNames=false, string formatString="F2" ) {
	
			// index into the unit names array; also the power of 1024 that represents the unit base.
			int unitIndex = 0;
			int maxUnits = DataSize.UnitPrefixes.Length - 1;
			long nextUnitSize = DataSize.UnitSizeFactor;


			while ( bytes > nextUnitSize && unitIndex <= maxUnits ) {

				nextUnitSize <<= DataSize.UnitShiftStep;
				unitIndex++;

			}

			/// nextUnitSize overshoots the actual units needed. baseUnits are the correct units at this point.

			double decimalVal;
			if ( unitIndex == 0 ) {

				return bytes.ToString();
			} else {
				decimalVal = ( (double)bytes ) / ( 1 << (unitIndex*UnitShiftStep) );
			}

			//Console.WriteLine( "base units: " + baseUnits );

			string decimalString = decimalVal.ToString( formatString );

			if ( fullUnitNames ) {
				return decimalString + DataSize.UnitNames[unitIndex] + DataSize.ByteWord;
			} else {
				return decimalString + DataSize.UnitPrefixes[unitIndex] + DataSize.ByteSymbol;
			}

		}

		public override string ToString() {
			return DataSize.ToDataString( this.bytes );
		}

		#region BOILER PLATE STRUCT CODE

		public override bool Equals( object obj ) {
			return ( obj is DataSize ) && this.bytes == ( (DataSize)obj ).bytes;
		}

		public bool Equals( DataSize other ) {
			return other.bytes == this.bytes;
		}

		public override int GetHashCode() {
			return this.bytes.GetHashCode();
		}

		public static bool operator ==( DataSize a, DataSize b ) {
			return a.bytes == b.bytes;
		}
		public static bool operator !=( DataSize a, DataSize b ) {
			return a.bytes != b.bytes;
		}

		public static implicit operator long( DataSize s ) {
			return s.bytes;
		}
		public static implicit operator DataSize( long l ) {
			return new DataSize( l );
		}

		public int CompareTo( DataSize other ) {
			if ( this.bytes < other.bytes ) {
				return -1;
			} else if ( other.bytes > this.bytes ) {
				return 1;
			}
			return 0;
		}

#endregion

	} // class

} // namespace