using System;
using Xamarin.Forms;
using MvvmHelpers;

namespace ImageGallery.Models
{
	public class GalleryImage : ObservableObject
	{
		public GalleryImage ()
		{
			ImageId = Guid.NewGuid ();
		}

		public Guid ImageId {
			get;
			set;
		}

		public ImageSource	Source {
			get;
			set;
		}

		public byte[] OrgImage {
			get;
			set;
		}
			
	}
}

