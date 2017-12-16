using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Lemur.Tasks;

namespace TorboFile.Operations {

	[Flags]
	public enum ImageCompareFlags {

		/// <summary>
		/// If set, only images with the same dimensions can be considered matching.
		/// </summary>
		MatchDimensions,

		/// <summary>
		/// If set, only images of the same format type can be considered matching.
		/// </summary>
		MatchFormat,

		/// <summary>
		/// If set, alpha channels are ignored when comparing images.
		/// </summary>
		IgnoreAlpha

	}

	/// <summary>
	/// TODO: Not yet implemented.
	/// </summary>
    public class ImageCompareOp : ProgressOperation {

		private BitmapSource leftSource;
		private BitmapSource rightSource;

		/// <summary>
		/// First image being compared.
		/// </summary>
		public BitmapSource LeftSource {
			get { return this.leftSource; }
			set { this.leftSource = value; }
		}

		/// <summary>
		/// Second image being compared.
		/// </summary>
		public BitmapSource RightSource {
			get { return this.rightSource; }
			set { this.rightSource = value; }
		}

		/// <summary>
		/// Maximum color error for each pixel when considering a match.
		/// </summary>
		public int MaxPixelError {
			get { return this.maxPixelError; }
			set { this.maxPixelError = value; }
		}
		int maxPixelError;

		/// <summary>
		/// Maximum total error between the images over all pixels.
		/// </summary>
		public long MaxTotalError {
			get { return this.maxTotalError; }
			set { this.maxTotalError = value; }
		}
		long maxTotalError;

		/// <summary>
		/// Current error during the operation of the comparison.
		/// </summary>
		//long currentError;
		//private ImageCompareFlags options;

		public override void Run() {
		}

	} // class

} // namespace
