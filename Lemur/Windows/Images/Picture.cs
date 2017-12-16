using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace Lemur.Windows.Images {

	/// <summary>
	/// Changed name to Picture to reduce conflicts with System.Drawing.Image.
	/// Must derive from FrameworkElement in order to use DrawingVisuals.
	/// </summary>
	public class Picture : FrameworkElement {

		/// <summary>
		/// Metadata query for retrieving frame delay time in a GIF.
		/// </summary>
		private const string GIF_DELAY_QUERY = "/grctlext/Delay";

		#region DEPENDENCY PROPERTIES

		/// <summary>
		/// Dependency Property for an image Uri Source.
		/// </summary>
		public static readonly DependencyProperty UriProperty = DependencyProperty.Register( "Uri", typeof( Uri ), typeof( Picture ),
			new FrameworkPropertyMetadata( null, new PropertyChangedCallback( OnUriChanged ) ) );

		/// <summary>
		/// D.P. Uri source of an image.
		/// </summary>
		public Uri Uri {
			get { return (Uri)this.GetValue( UriProperty ); }
			set {
				this.SetValue( UriProperty, value );
			}
		}

		private static void OnUriChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
			( (Picture)d ).RefreshSource();
		}

		/// <summary>
		/// Dependency Property for the currently displayed frame of an image.
		/// </summary>
		public static readonly DependencyProperty FrameProperty =
			DependencyProperty.Register( "Frame", typeof( int ), typeof( Picture ), new FrameworkPropertyMetadata( 1,
				FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback( OnFrameChanged ) ) );

		/// <summary>
		/// Index of the current picture frame being displayed.
		///	This is the accessor for the FrameProperty dependency property.
		/// </summary>
		public int Frame {
			get { return (int)GetValue( FrameProperty ); }
			set { this.SetValue( FrameProperty, value ); }
		}

		/// <summary>
		/// Callback for when the Dependency Property FrameProperty changes.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnFrameChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
			///Console.WriteLine( "frame changed: " + e.NewValue );
			( (Picture)d ).DrawFrame( (int)e.NewValue );

		}

		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register( "Stretch",
			typeof( Stretch ), typeof( Picture ), new FrameworkPropertyMetadata( Stretch.Uniform,
				FrameworkPropertyMetadataOptions.AffectsMeasure ) );

		public Stretch Stretch {
			get { return (Stretch)this.GetValue( StretchProperty ); }
			set { this.SetValue( StretchProperty, value ); }
		}

		public static readonly DependencyProperty FrameCountProperty =
			DependencyProperty.Register( "FrameCount", typeof( int ), typeof( Picture ),
				new FrameworkPropertyMetadata( 1, FrameworkPropertyMetadataOptions.NotDataBindable ) );

		/// <summary>
		/// FrameCount cannot be set, but can be read to Bind to other properties.
		/// </summary>
		public int FrameCount {
			get { return (int)GetValue( FrameCountProperty ); }
			private set { this.SetValue( FrameCountProperty, value ); }
		}

#endregion

		/// <summary>
		/// Frames of the currently loaded image.
		/// </summary>
		private BitmapFrame[] _currentFrames;

		/// <summary>
		/// Clock controlling the current animation, if any.
		/// </summary>
		private AnimationClock _clock;


		#region FRAMEWORK LIFE EVENTS

		public Picture() {

			this.Unloaded += Picture_Unloaded;
			this.Loaded += Picture_Loaded;

			this.InitPictureVisual();

		}


		private void Picture_Loaded( object sender, RoutedEventArgs e ) {

			this.SizeChanged += Picture_SizeChanged;

		}

		private void Picture_SizeChanged( object sender, SizeChangedEventArgs e ) {
			Console.WriteLine( "PICTURE SIZE CHANGED: "  + e.NewSize );
			this.Repaint();
		}

		private void Picture_Unloaded( object sender, RoutedEventArgs e ) {

			Console.WriteLine( "PICTURE UNLOADED" );
			this.StopAnimation();

			if( this._clock != null ) {
				this._clock.Controller.Remove();
				this._clock = null;
			}

			this._currentFrames = null;
			this.SizeChanged -= this.Picture_SizeChanged;
		}

		#endregion


		#region SOURCE IMAGE LOADING

		public async void RefreshSource() {

			Uri cur = this.Uri;
			this.ClearPicture();

			if( cur == null || string.IsNullOrEmpty( cur.LocalPath ) ) {

				Console.WriteLine( "URI IS NULL OR EMPTY" );
				return;
			}
			Console.WriteLine( "LOADING IMAGE: " + cur.LocalPath );
			await this.LoadImageAsync( cur.LocalPath );

		}

		/// <summary>
		/// Attempts to load an image asynchronously.
		/// </summary>
		/// <param name="imagePath"></param>
		/// <returns></returns>
		private async Task LoadImageAsync( string imagePath ) {

			if( string.IsNullOrEmpty( imagePath ) || !System.IO.File.Exists( imagePath ) ) {
				return;
			}

			BitmapDecoder decoder = await Dispatcher.InvokeAsync( () => {

				FileInfo f = new FileInfo( imagePath );

				using( FileStream stream = System.IO.File.OpenRead( imagePath ) ) {
					return BitmapDecoder.Create( stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad );
				}

			} );

			ICollection<BitmapFrame> frames = decoder.Frames;

			Console.WriteLine( "IMAGE LOADED" );

			if( frames != null ) {

				this.SetImageFrames( frames );

			} else {

				Console.WriteLine( "ERROR: No Decoded frames." );
				this.ClearPicture();
			}

		} // LoadImageAsync()

		/// <summary>
		/// Sets the Bitmap frames that will be used to display the Picture.
		/// </summary>
		/// <param name="frames"></param>
		/// <param name="freezeFrames">Whether to Freeze() the provided ImageSources before use.</param>
		private void SetImageFrames( ICollection<BitmapFrame> frames, bool freezeFrames=true ) {

			if( frames == null || frames.Count == 0 ) {

				Console.WriteLine( "NO FRAMES FOUND" );
				this.FrameCount = 0;
				this._currentFrames = new BitmapFrame[0];
				return;

			}

			this.CopyAndFreeze( frames, freezeFrames );

			/**
			 * NOTE: Currently only supports all images being the same size.
			*/
			BitmapFrame curFrame = this._currentFrames[0];

			if( !TryGetImageSize( out this._loadedImageSize, 0 ) ) {
				Console.WriteLine( "NO VALID IMAGE SIZE" );
				this._loadedImageSize = new Size( this.ActualWidth, this.ActualHeight );
			} else {
				Console.WriteLine( "SETTING IMAGE SIZE" );
			}

			//Console.WriteLine( "PIXLE HEIGHTS: " + curFrame.PixelWidth + "," + curFrame.PixelHeight );
			//Console.WriteLine( "IMAGE SIZE: " + curFrame.Width + " ," + curFrame.Height );

			this._invalidMeasure = true;
			this.InvalidateMeasure();

			this.Frame = 0;

			if( this._currentFrames.Length > 1 ) {
				Console.WriteLine( "STARTING ANIMATION" );
				this.InitAnimation();
			}

		} //

		/// <summary>
		/// Copies and freezes the frames into the current frames.
		/// </summary>
		/// <param name="frames"></param>
		/// <param name="freezeFrames"></param>
		private void CopyAndFreeze( ICollection<BitmapFrame> frames, bool freezeFrames ) {

			int count = frames.Count;

			this._currentFrames = new BitmapFrame[count];
			frames.CopyTo( this._currentFrames, 0 );
			this.FrameCount = count;

			if( freezeFrames ) {
				foreach( BitmapFrame f in frames ) {

					if( !f.IsFrozen && f.CanFreeze ) {
						f.Freeze();
					}
				}
			}

		} // CopyAndFreeze()

		#endregion SOURCE LOADING

		#region DRAWING/DISPLAY CODE

		private Size _loadedImageSize;
		private Rect _displayRect;

		protected override Size MeasureOverride( Size availableSize ) {

			/*Size imgSize;
			if( !TryGetImageSize( out imgSize )) {
				return base.MeasureOverride( availableSize );
			}
			return imgSize;*/
			return this._loadedImageSize;

		}

		/// <summary>
		/// True on first draw with correct metrics after an image has been
		/// loaded.
		/// </summary>
		private bool _invalidMeasure;

		protected override Size ArrangeOverride( Size finalSize ) {

			Size imgSize = this._loadedImageSize;
			/*if( !TryGetImageSize( out imgSize ) || imgSize.Height == 0 || imgSize.Width == 0 ||
				finalSize.Width == 0 || finalSize.Height == 0 ) {
				return base.ArrangeOverride( finalSize );
			}*/
			if( this._invalidMeasure ) {
				Console.WriteLine( "Measure was invalid" );
				this._invalidMeasure = false;
			}

			this._displayRect = new Rect( 0, 0, imgSize.Width, imgSize.Height );
			//Console.WriteLine( "ARRANGE OVERRIDE Displayrect: " + this._displayRect );

			this.DrawFrame( this.Frame );
			return imgSize;

		}

		/*protected override Size MeasureOverride( Size availableSize ) {

			Size imgSize;
			if( !TryGetImageSize( out imgSize ) || imgSize.Height == 0 || imgSize.Width == 0 ||
				availableSize.Width == 0 || availableSize.Height == 0 ) {
				return base.MeasureOverride( availableSize );
			}

			switch( this.Stretch ) {
				case Stretch.None:
					return imgSize;
				case Stretch.Fill:
					return availableSize;
				case Stretch.Uniform:

					// scale to fit both dimensions in bounds.
					double fitScale = Math.Min( availableSize.Width / imgSize.Width, availableSize.Height / imgSize.Height );
					if( fitScale >= 1 ) {
						// image already fits uniformly.
						return imgSize;
					}

					/// use smallest scaling that will fit both dimensions.
					return new Size( fitScale * imgSize.Width, fitScale * imgSize.Height );

				case Stretch.UniformToFill:

					// available size will be used and filled regardless; so the available size is correct?
					return availableSize;

			} // switch()

			return imgSize;

		} // MeasureOverride()*/

		/*protected override Size ArrangeOverride( Size finalSize ) {

			Console.WriteLine( "ARRANGE CALLED" );

			Size imgSize;
			if( !TryGetImageSize( out imgSize ) ) {
				this._displayRect = new Rect( 0, 0, finalSize.Width, finalSize.Height );
				return base.ArrangeOverride( finalSize );
			}

			switch( this.Stretch ) {
				case Stretch.None:

					this._displayRect = new Rect( 0, 0, imgSize.Width, imgSize.Height );
					break;

				case Stretch.Fill:

					this._displayRect = new Rect( 0, 0, finalSize.Width, finalSize.Height );
					break;

				case Stretch.Uniform:

					// scale to fit both dimensions in bounds.
					double fitScale = Math.Min( finalSize.Width / imgSize.Width, finalSize.Height / imgSize.Height );
					if( fitScale >= 1 ) {
						// image already fits uniformly.
						this._displayRect = new Rect( 0, 0, imgSize.Width, imgSize.Height );
					} else {
						/// use smallest scaling that will fit both dimensions.
						this._displayRect = new Rect( 0, 0, fitScale * imgSize.Width, fitScale * imgSize.Height );
					}
					break;

				case Stretch.UniformToFill:

					double fillScale = Math.Min( finalSize.Width / imgSize.Width, finalSize.Height / imgSize.Height );
					this._displayRect = new Rect( 0, 0, fillScale * imgSize.Width, fillScale * imgSize.Height );
					break;

			} // switch()

			return new Size( this._displayRect.Width, this._displayRect.Height );

		} // ArrangeOverride()*/

		private void SetDisplayRect( Size imgSize, Size layoutSize ) {
		}

		/// <summary>
		/// Display the frame index for the current picture.
		/// </summary>
		/// <param name="index"></param>
		public void DrawFrame( int index ) {

			if( this._currentFrames == null || this._invalidMeasure ) {
				/// When an image is first loaded, it has to wait for MeasureOverride to be called
				/// with the correct image size.
				return;
			}

			if( index < 0 || index >= this.FrameCount ) {
				Console.WriteLine( "INVALID INDEX: " + index );
				return;
			}

			BitmapFrame frame = this._currentFrames[index];

			if( frame == null ) {
				throw new NullReferenceException( "Indexed BitmapFrame is null." );
			}

			this.DrawToContainer( frame );

		} //

		/// <summary>
		/// Draws the current BitmapFrame into the picture DrawingVisual.
		/// </summary>
		/// <param name="source"></param>
		private void DrawToContainer( ImageSource source ) {

			if( _picContainer.Drawing != null ) {

				DrawingContext context = this._picContainer.RenderOpen();
				context.DrawDrawing( _picContainer.Drawing );
				context.DrawImage( source, this._displayRect );
				 
				context.Close();

			} else {

				DrawingContext context = this._picContainer.RenderOpen();
			
				context.DrawImage( source, this._displayRect );

				context.Close();
			}
		}

		/// <summary>
		/// Repaints the existing image at a new size.
		/// </summary>
		private void Repaint() {

			if( _picContainer.Drawing != null ) {

				DrawingContext context = this._picContainer.RenderOpen();
				context.DrawDrawing( _picContainer.Drawing );
				context.Close();

			}

		}

		/// <summary>
		/// If an image has been loaded and is valid, returns the natural size of the image
		/// in the out variable and returns true.
		/// If no image is available or no size is valid, returns false;
		/// </summary>
		/// <param name="imgSize"></param>
		/// <returns></returns>
		public bool TryGetImageSize( out Size imgSize, int index ) {

			if( index < 0 || this._currentFrames == null || index >= this._currentFrames.Length ) {
				imgSize = new Size();
				return false;
			}
			BitmapFrame frame = this._currentFrames[index];
			if( frame == null ) {
				imgSize = new Size();
				return false;
			}

			imgSize = new Size( frame.PixelWidth, frame.PixelHeight );
			return true;

		} //

		/// <summary>
		/// Removes the previous image from the drawing container.
		/// </summary>
		private void ClearPicture() {

			this.StopAnimation();

			if( this._picContainer != null ) {
				DrawingContext context = this._picContainer.RenderOpen();
				context.Close();
			}

		}

#endregion

		#region ANIMATION CONTROL

		private void InitAnimation() {

			BitmapFrame frame = this._currentFrames[0];

			AnimationTimeline anim;
			if( frame.Decoder is GifBitmapDecoder ) {

				anim = BuildGIFAnimation( this._currentFrames );

			} else {
				anim = BuildFrameAnimation( this._currentFrames );

			} //

			anim.RepeatBehavior = RepeatBehavior.Forever;
			anim.Freeze();

			this._clock = anim.CreateClock();
			this.ApplyAnimationClock( FrameProperty, this._clock );


		} //

		/// <summary>
		/// Returns delay between frames in milliseconds.
		/// </summary>
		/// <param name="frame"></param>
		/// <returns></returns>
		private int GetFrameDelay( BitmapFrame frame ) {

			object data = ( (BitmapMetadata)frame.Metadata ).GetQuery( GIF_DELAY_QUERY );
			if( Equals( data, null ) ) {
				//Console.WriteLine( "NO DELAY METADATA" );
				return 0;
			}
			//Console.WriteLine( "DATA IS TYPE: " + data.GetType().ToString() );
			if( ( data is ushort ) || ( data is int ) ) {
				return (int)( 10 * (ushort)data );
			}
			return 0;

		}

		private void StopAnimation() {

			if( this._clock != null ) {
				this._clock.Controller.Remove();
			}

		}


		private AnimationTimeline BuildGIFAnimation( BitmapFrame[] frames ) {

			Int32AnimationUsingKeyFrames anim = new Int32AnimationUsingKeyFrames();

			// check base metadata???
			int len = frames.Length - 1;
			int totalTime = 0;
			for( int i = 0; i <= len; i++ ) {

				anim.KeyFrames.Add(
					new LinearInt32KeyFrame( i, KeyTime.FromTimeSpan( new TimeSpan( 0, 0, 0, 0, totalTime ) ) ) );

				totalTime += this.GetFrameDelay( frames[i] );

			} // for-loop.

			// final frame must be held for its own hold time.
			anim.KeyFrames.Add(
					new LinearInt32KeyFrame( len, KeyTime.FromTimeSpan( new TimeSpan( 0, 0, 0, 0, totalTime ) ) ) );


			return anim;

		}

		/// <summary>
		/// Create an AnimationTimeline based on the number of keyframes in an image,
		/// without any duration specified per frame.
		/// </summary>
		/// <returns></returns>
		private AnimationTimeline BuildFrameAnimation( BitmapFrame[] frames, double secPerFrame=0.1 ) {

			int len = frames.Length;

			Int32Animation frameAnimation = new Int32Animation( 0, len-1,
				new Duration( TimeSpan.FromSeconds( len*secPerFrame ) )
			);
			frameAnimation.RepeatBehavior = RepeatBehavior.Forever;

			return frameAnimation;

		} //

		#endregion


		#region PICTURE CONTAINER

		private DrawingVisual _picContainer;
		private void InitPictureVisual() {

			DrawingVisual drawing = new DrawingVisual();

			this.AddVisualChild( drawing );
			this._picContainer = drawing;

			//this.RegisterName( "__Pic", this._picContainer );

		} //

		/// <summary>
		/// Only one visual child; the picture container.
		/// </summary>
		protected override int VisualChildrenCount => this._picContainer == null ? 0 : 1;

		protected override Visual GetVisualChild( int index ) {

			if( this._picContainer == null || index != 0 ) {
				throw new ArgumentOutOfRangeException();
			}
			return this._picContainer;

		}

		#endregion

		#region DEBUG

		[Conditional("DEBUG")]
		private void TraceCodecInfo( BitmapCodecInfo codec ) {

			Console.WriteLine( "friendlyname: " + codec.FriendlyName );
			Console.WriteLine( "extensions: " + codec.FileExtensions );
			Console.WriteLine( "version: " + codec.Version);
			Console.WriteLine( "mimes: " + codec.MimeTypes );
			Console.WriteLine( "author: " + codec.Author);

		}

		/// <summary>
		/// Used to figure out the transparency issue. Palette contains
		/// non-opaque 'black' color.
		/// </summary>
		/// <param name="source"></param>
		[Conditional("DEBUG")]
		private void TracePalette( BitmapSource source ) {

			foreach( Color c in source.Palette.Colors ) {
				Console.WriteLine( "Color: " + c );
			}
	
		}

		/// <summary>
		/// Traces information about a BitmapSource PixelFormat.
		/// </summary>
		/// <param name="source"></param>
		[Conditional( "DEBUG" )]
		private void TraceFormat( BitmapSource source ) {

			PixelFormat format = source.Format;
			if( format == null ) {
				Console.WriteLine( "PixelFormat null" );
				return;
			}

			Console.WriteLine( "BitsPerPixel: " + format.BitsPerPixel );

			IList<PixelFormatChannelMask> masks = format.Masks;
			if( masks == null ) {
				Console.WriteLine( "No Pixel Masks" );
				return;
			}

			Console.WriteLine( "Mask count: " + masks.Count );
			foreach( PixelFormatChannelMask mask in masks ) {

				foreach( byte b in mask.Mask ) {
					Console.WriteLine( "Mask: " + b.ToString() );
				}

			}

		}

#endregion

	} // class

} // namespace
