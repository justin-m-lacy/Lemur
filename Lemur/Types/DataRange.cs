using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace Lemur.Types {

	//[SettingsSerializeAs(SettingsSerializeAs.String)]
	[TypeConverter(typeof(DataRangeConverter))]
	public struct DataRange : IEquatable<DataRange> {

		private DataSize minSize;
		public DataSize MinSize {
			get { return this.minSize; }
			set { this.minSize = value; }
		}

		private DataSize maxSize;
		public DataSize MaxSize {
			get { return this.maxSize; }
			set {
				this.maxSize = value;
			}
		}

		/*public DataRange() {
			this.minSize = new DataSize();
			this.maxSize = new DataSize();
		}*/

		public DataRange( long min, long max ) {

			this.minSize = new DataSize( min );
			this.maxSize = new DataSize( max );

		}

		public DataRange( DataSize min, DataSize max ) {

			this.minSize = min;
			this.maxSize = max;

		}

		/// <summary>
		/// Initializes a DataRange with a lower bound of 0 bytes.
		/// </summary>
		/// <param name="upperSize"></param>
		public DataRange( DataSize upperSize ) {
			this.minSize = new DataSize();
			this.maxSize = new DataSize( upperSize );
		}

		#region struct code

		public bool Contains( long size ) {
			return size >= this.minSize && size <= this.maxSize;
		}

		public override bool Equals( object obj ) {

			if( !(obj is DataRange) ) {
				return false;
			}
			DataRange other = (DataRange)obj;
			return ( other.minSize == this.minSize && other.maxSize == this.maxSize );
		}


		public override int GetHashCode() {
			return this.minSize.GetHashCode() ^ this.maxSize.GetHashCode();
		}

		public static bool operator ==( DataRange a, DataRange b ) {
			if( a == null && b == null ) {
				return true;
			}

			return a.minSize == b.minSize && a.maxSize == b.maxSize;
		}
		public static bool operator !=( DataRange a, DataRange b ) {

			if( ( a == null && b != null ) || ( b == null && a != null ) ) {
				return false;
			}

			return (a.minSize != b.minSize) || (a.maxSize != b.maxSize );
		}

		public bool Equals( DataRange other ) {
			/*if( other == null ) {
				return false;
			}*/
			return this.minSize == other.minSize && this.maxSize == other.maxSize;

		} //

#endregion

	} // class

} // namespace
