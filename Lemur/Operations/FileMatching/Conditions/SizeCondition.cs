using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Size Test", "Allows matching files by file size." )]
	[Serializable]
	public class SizeCondition : BaseCondition {

		private DataSize _minSize;
		/// <summary>
		/// Minimum Size of matched file.
		/// </summary>
		public DataSize MinSize {
			get { return this._minSize; }
			set {
				this._minSize = value;
			}
		}

		private DataSize _maxSize;

		/// <summary>
		/// Maximum size of matched file.
		/// </summary>
		public DataSize MaxSize {
			get { return this._maxSize; }
			set {
				this._maxSize = value;
			}
		}

		public SizeCondition() { }

		public SizeCondition( DataSize min, DataSize max ) {
			this.MinSize = min;
			this.MaxSize = max;
		}

		public SizeCondition( long minSize, long maxSize ) {

			this.MinSize = minSize;
			this.MaxSize = maxSize;

		}

		public override bool IsMatch( FileSystemInfo info ) {

			FileInfo fInfo = info as FileInfo;
			if( fInfo == null ) {
				// Exclude all directories regardless of include/exclude.
				return false;
			}
			long fileSize = fInfo.Length;

			bool inRange = ( this._minSize <= fileSize && fileSize <= this._maxSize );
			return base.IsMatch( inRange );

		} // IsMatch()

	} // class

} // namespace
