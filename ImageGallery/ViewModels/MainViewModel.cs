using System;
using MvvmHelpers;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using System.Threading.Tasks;
using System.IO;
using ImageGallery.Models;
using System.Collections.ObjectModel;
using Plugin.Media.Abstractions;
using System.Linq;
using ImageGallery.Services;

namespace ImageGallery.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		ICommand _cameraCommand, _previewImageCommand = null;
		ObservableCollection<GalleryImage> _images = new ObservableCollection<GalleryImage> ();
		ImageSource _previewImage = null;


		public MainViewModel ()
		{			
		}

        public ObservableCollection<GalleryImage> Images => _images;
        public ImageSource PreviewImage {
            get => _previewImage;
            set => SetProperty(ref _previewImage, value);
        }

        public ICommand CameraCommand => _cameraCommand ?? 
            new Command(async () => await ExecuteCameraCommand(), () => CanExecuteCameraCommand());

        public bool CanExecuteCameraCommand ()
		{
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) {
				return false;
			}
			return true;
		}

		public async Task ExecuteCameraCommand ()
		{
			var file = await CrossMedia.Current.TakePhotoAsync (new Plugin.Media.Abstractions.StoreCameraMediaOptions { PhotoSize = PhotoSize.Small });

			if (file == null)
				return;


			byte[] imageAsBytes = null;
			using (var memoryStream = new MemoryStream ()) {
				file.GetStream ().CopyTo (memoryStream);
				file.Dispose ();
				imageAsBytes = memoryStream.ToArray ();
			}

			var resizer = DependencyService.Get<IImageResize> ();

			imageAsBytes = resizer.ResizeImage (imageAsBytes, 1080, 1080);

			var imageSource = ImageSource.FromStream (() => new MemoryStream (imageAsBytes));

			_images.Add (new GalleryImage{ Source = imageSource, OrgImage = imageAsBytes });


			return;
		}

        public ICommand PreviewImageCommand => _previewImageCommand ?? 
            new Command<Guid>((img) =>
            {

                var image = _images.Single(x => x.ImageId == img).OrgImage;

                PreviewImage = ImageSource.FromStream(() => new MemoryStream(image));

            });
     }
}
