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

        public IList<View> Children => _imageStack.Children;


        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource), 
                typeof(IList), 
                typeof(ImageGallery),
                default(IList),
                BindingMode.TwoWay,
                propertyChanging: (bindable, oldValue, newValue) =>
                {
                    ((ImageGallery)bindable).ItemsSourceChanging();
                },
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((ImageGallery)bindableObject).ItemsSourceChanged(bindableObject, (IList)oldValue, (IList)newValue);
            });

        public IList ItemsSource {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
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
                            if (view is BindableObject bindableObject)
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
              BindableProperty.Create(
                  nameof(SelectedItem), 
                  typeof(object), 
                  typeof(ImageGallery),
                  null,
                  BindingMode.TwoWay,
                  propertyChanged: (bindable, oldValue, newValue) => {
                      ((ImageGallery)bindable).UpdateSelectedIndex();
                  }
              );

        public object SelectedItem {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
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
            BindableProperty.Create(
                nameof(SelectedIndex),
                typeof(int),
                typeof(ImageGallery),
                0,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((ImageGallery)bindable).UpdateSelectedItem();
                });

        public int SelectedIndex {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        void UpdateSelectedItem() => 
            SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;
    }
}

