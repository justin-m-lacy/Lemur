using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Composition;
using System.Windows.Media.Imaging;

namespace Lemur.Windows.Images {

	/// <summary>
	/// CURRENTLY UNUSED.
	/// </summary>
	public class AnimatedBitmap : BitmapSource {

		public static readonly DependencyProperty FrameProperty =
			DependencyProperty.Register( "Frame", typeof( int ), typeof( AnimatedBitmap ), new FrameworkPropertyMetadata( 1 ,
				FrameworkPropertyMetadataOptions.AffectsRender) );

		public static readonly DependencyProperty FrameCountProperty =
			DependencyProperty.Register( "FrameCount", typeof( int ), typeof( AnimatedBitmap ),
				new FrameworkPropertyMetadata( 1, FrameworkPropertyMetadataOptions.None ) );

		public int Frame {
			get { return (int)GetValue( FrameProperty ); }
			set { this.SetValue( FrameProperty, value ); }
		}
		public int FrameCount {
			get { return (int)GetValue( FrameCountProperty ); }
			private set { this.SetValue( FrameCountProperty, value ); }
		}

		protected override Freezable CreateInstanceCore() {
			return new AnimatedBitmap();
		}

		public AnimatedBitmap() {

		}

	} // class

} // namespace