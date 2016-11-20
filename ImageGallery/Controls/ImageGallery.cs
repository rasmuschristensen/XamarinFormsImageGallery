using System;
using Xamarin.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;

namespace ImageGallery.Controls
{
	public class ImageGallery : ScrollView
	{
		readonly StackLayout _imageStack;

		public ImageGallery ()
		{
			this.Orientation = ScrollOrientation.Horizontal;

			_imageStack = new StackLayout {
				Orientation = StackOrientation.Horizontal
			};

			this.Content = _imageStack;
		}

		public IList<View> Children {
			get {
				return _imageStack.Children;
			}
		}


		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create<ImageGallery, IList> (
				view => view.ItemsSource,
				default(IList), 
				BindingMode.TwoWay,
				propertyChanging: (bindableObject, oldValue, newValue) => {
					((ImageGallery)bindableObject).ItemsSourceChanging ();
				},
				propertyChanged: (bindableObject, oldValue, newValue) => {
					((ImageGallery)bindableObject).ItemsSourceChanged (bindableObject, oldValue, newValue);
				}
			);

		public IList ItemsSource {
			get {
				return (IList)GetValue (ItemsSourceProperty);
			}
			set {

				SetValue (ItemsSourceProperty, value);
			}
		}

		void ItemsSourceChanging ()
		{
			if (ItemsSource == null)
				return;			
		}

		void ItemsSourceChanged (BindableObject bindable, IList oldValue, IList newValue)
		{		
			if (ItemsSource == null)
				return;

			var notifyCollection = newValue as INotifyCollectionChanged;
			if (notifyCollection != null) {
				notifyCollection.CollectionChanged += (sender, args) => {
					if (args.NewItems != null) {
						foreach (var newItem in args.NewItems) {

							var view = (View)ItemTemplate.CreateContent ();
							var bindableObject = view as BindableObject;
							if (bindableObject != null)
								bindableObject.BindingContext = newItem;
							_imageStack.Children.Add (view);
						}
					}
					if (args.OldItems != null) {
						// not supported
						_imageStack.Children.RemoveAt (args.OldStartingIndex);
					}
				};
			}
				
		}

		public DataTemplate ItemTemplate {
			get;
			set;
		}

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create<ImageGallery, object> (
				view => view.SelectedItem,
				null,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
					((ImageGallery)bindable).UpdateSelectedIndex ();
				}
			);

		public object SelectedItem {
			get {
				return GetValue (SelectedItemProperty);
			}
			set {
				SetValue (SelectedItemProperty, value);
			}
		}

		void UpdateSelectedIndex ()
		{
			if (SelectedItem == BindingContext)
				return;

			SelectedIndex = Children
				.Select (c => c.BindingContext)
				.ToList ()
				.IndexOf (SelectedItem);

		}

		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create<ImageGallery, int> (
				carousel => carousel.SelectedIndex,
				0,
				BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
					((ImageGallery)bindable).UpdateSelectedItem ();
				}
			);

		public int SelectedIndex {
			get {
				return (int)GetValue (SelectedIndexProperty);
			}
			set {
				SetValue (SelectedIndexProperty, value);
			}
		}

		void UpdateSelectedItem ()
		{
			SelectedItem = SelectedIndex > -1 ? Children [SelectedIndex].BindingContext : null;
		}
	}
}

