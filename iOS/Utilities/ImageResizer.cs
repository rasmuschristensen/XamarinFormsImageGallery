using System;
using ImageGallery.Services;
using UIKit;
using CoreGraphics;
using System.Drawing;
using ImageGallery.iOS.Utilities;

[assembly: Xamarin.Forms.Dependency (typeof(ImageResizer))]
namespace ImageGallery.iOS.Utilities
{
	public class ImageResizer : IImageResize
	{
		public ImageResizer ()
		{
		}

		#region IImageResize implementation


		public byte[] ResizeImage (byte[] imageData, float width, float height)
		{
			UIImage originalImage = ImageFromByteArray (imageData);
			UIImageOrientation orientation = originalImage.Orientation;

			//create a 24bit RGB image
			using (CGBitmapContext context = new CGBitmapContext (IntPtr.Zero,
				                                 (int)width, (int)height, 8,
				                                 (int)(4 * width), CGColorSpace.CreateDeviceRGB (),
				                                 CGImageAlphaInfo.PremultipliedFirst)) {

				RectangleF imageRect = new RectangleF (0, 0, width, height);

				// draw the image
				context.DrawImage (imageRect, originalImage.CGImage);

				UIKit.UIImage resizedImage = UIKit.UIImage.FromImage (context.ToImage (), 0, orientation);

				// save the image as a jpeg
				return resizedImage.AsJPEG ().ToArray ();
			}
		}

		public static UIKit.UIImage ImageFromByteArray (byte[] data)
		{
			if (data == null) {
				return null;
			}

			UIKit.UIImage image;
			try {
				image = new UIKit.UIImage (Foundation.NSData.FromArray (data));
			} catch (Exception e) {
				Console.WriteLine ("Image load failed: " + e.Message);
				return null;
			}
			return image;
		}
	}

	#endregion

}

