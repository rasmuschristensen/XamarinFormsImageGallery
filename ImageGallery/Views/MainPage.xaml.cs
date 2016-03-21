using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ImageGallery.ViewModels;

namespace ImageGallery.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

			BindingContext = new MainViewModel ();
		}
	}
}

