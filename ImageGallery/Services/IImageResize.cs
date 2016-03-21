using System;

namespace ImageGallery.Services
{
	public interface IImageResize
	{
		byte[] ResizeImage (byte[] imageData, float width, float height);
	}
}

