using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	public class SizeCondition : BaseCondition {

		private long _minSize;
		/// <summary>
		/// Minimum Size of matched file.
		/// </summary>
		public long MinSize {
			get { return this._minSize; }
			set {
				this._minSize = value;
			}
		}

		private long _maxSize;

		/// <summary>
		/// Maximum size of matched file.
		/// </summary>
		public long MaxSize {
			get { return this._maxSize; }
			set {
				this._maxSize = value;
			}
		}

		public SizeCondition( long minSize, long maxSize ) {

			this.MinSize = minSize;
			this.MaxSize = maxSize;

		}

		public SizeCondition() {
		}

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

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
