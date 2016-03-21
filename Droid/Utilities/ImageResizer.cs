using System;
using ImageGallery.Services;
using ImageGallery.Droid.Utilities;
using Android.Graphics;
using System.IO;

[assembly: Xamarin.Forms.Dependency (typeof(ImageResizer))]
namespace ImageGallery.Droid.Utilities
{
	public class ImageResizer : IImageResize
	{
		public ImageResizer ()
		{
		}

		public byte[] ResizeImage (byte[] imageData, float width, float height)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray (imageData, 0, imageData.Length);
			Bitmap resizedImage = Bitmap.CreateScaledBitmap (originalImage, (int)width, (int)height, false);

			using (MemoryStream ms = new MemoryStream ()) {
				resizedImage.Compress (Bitmap.CompressFormat.Jpeg, 100, ms);
				return ms.ToArray ();
			}
		}
			
	}
}

