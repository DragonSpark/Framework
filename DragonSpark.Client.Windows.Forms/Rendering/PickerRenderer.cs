using DragonSpark.Extensions;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;
using DataTemplate = System.Windows.DataTemplate;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class PickerRenderer : ViewRenderer<Picker, ComboBox>
	{
		class ItemViewModel : INotifyPropertyChanged
		{
			string data;
			// float opacity;
			int maxHeight;
			public event PropertyChangedEventHandler PropertyChanged = delegate { };

			public int MaxHeight
			{
				get { return maxHeight; }
				set
				{
					if ( value == maxHeight )
					{
						return;
					}
					maxHeight = value;
					PropertyChanged( this, new PropertyChangedEventArgs( "MaxHeight" ) );
				}
			}

			public string Data
			{
				get { return data; }
				set
				{
					if ( value == data )
					{
						return;
					}
					data = value;
					PropertyChanged( this, new PropertyChangedEventArgs( "Data" ) );
				}
			}

			/*public float Opacity
			{
				get { return opacity; }
				set
				{
					if ( value == opacity )
					{
						return;
					}
					opacity = value;
					PropertyChanged( this, new PropertyChangedEventArgs( "Opacity" ) );
				}
			}*/

			public ItemViewModel( string item )
			{
				// opacity = 1f;
				data = item;
				maxHeight = 2147483647;
			}
		}

		class PickerTracker : VisualElementTracker<Picker, FrameworkElement>
		{
			protected override void LayoutChild()
			{
				Child.Width = Model.Width - 24.0;
				Child.Height = Model.Height;
			}
		}

		bool isChanging;

		protected override void OnElementChanged( ElementChangedEventArgs<Picker> e )
		{
			var listPicker = new ComboBox { IsEditable = true, IsReadOnly =  true, Text = Element.Title };
			Tracker = new PickerTracker
			{
				Model = Element,
				Element = this
			};
			base.OnElementChanged( e );
			if ( e.OldElement != null )
			{
				( (ObservableList<string>)Element.Items ).CollectionChanged -= ItemsCollectionChanged;
			}
			( (ObservableList<string>)Element.Items ).CollectionChanged += ItemsCollectionChanged;
			listPicker.ItemTemplate = (DataTemplate)System.Windows.Application.Current.Resources["PickerItemTemplate"];
			/*listPicker.FullModeItemTemplate = (DataTemplate)Application.Current.Resources["PickerFullItemTemplate"];
			listPicker.ExpansionMode = ExpansionMode.FullScreenOnly;*/
			listPicker.Items.Add( new ItemViewModel( " " )
			{
				MaxHeight = 0
			} );
			/*System.Windows.Data.Binding binding = new System.Windows.Data.Binding("ListPickerMode")
			{
				Source = listPicker
			};
			DependencyProperty dp = DependencyProperty.RegisterAttached("ListPickerModeChanged", typeof(ComboBoxMo), typeof(ListPicker), new PropertyMetadata(new PropertyChangedCallback(this.ListPickerModeChanged)));
			listPicker.SetBinding(dp, binding);*/
			SetNativeControl( listPicker );
			UpdatePicker();
			listPicker.SelectionChanged += PickerSelectionChanged;
		}

		/*private void ListPickerModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue == null || e.NewValue == null)
			{
				return;
			}
			ListPickerMode listPickerMode = (ListPickerMode)e.OldValue;
			ListPickerMode listPickerMode2 = (ListPickerMode)e.NewValue;
			if (listPickerMode == ListPickerMode.Normal && listPickerMode2 == ListPickerMode.Full)
			{
				((IElementController)base.Element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
				return;
			}
			if (listPickerMode == ListPickerMode.Full && listPickerMode2 == ListPickerMode.Normal)
			{
				((IElementController)base.Element).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
			}
		}*/
		protected override void OnGotFocus(object sender, RoutedEventArgs args)
		{
		}
		protected override void OnLostFocus(object sender, RoutedEventArgs args)
		{
		}

		void ItemsCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			UpdateItems();
		}

		void UpdateItems()
		{
			isChanging = true;
			while ( Control.Items.Count < Element.Items.Count + 1 )
			{
				Control.Items.Add( new ItemViewModel( string.Empty ) );
			}
			while ( Control.Items.Count > Element.Items.Count + 1 )
			{
				Control.Items.RemoveAt( Control.Items.Count - 1 );
			}
			for ( var i = 0; i < Element.Items.Count; i++ )
			{
				var itemViewModel = (ItemViewModel)Control.Items[i + 1];
				if ( itemViewModel.Data != Element.Items[i] )
				{
					itemViewModel.Data = Element.Items[i];
				}
			}
			Control.SelectedIndex = Element.SelectedIndex + 1;
			isChanging = false;
		}

		void UpdatePicker()
		{
			Control.Text = Element.Title; // TODO: Might not work.
			UpdateItems();
			Control.SelectedIndex = Element.SelectedIndex + 1;
		}

		void PickerSelectionChanged( object sender, SelectionChangedEventArgs e )
		{
			if ( isChanging )
			{
				return;
			}
			sender.As<ComboBox>( item =>
			{
				if ( item.SelectedIndex != -1 )
				{
					var num = item.SelectedIndex - 1;
					( (IElementController)Element ).SetValueFromRenderer( Picker.SelectedIndexProperty, num );
				}
			} );
		}

		protected override void OnElementPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			base.OnElementPropertyChanged( sender, e );
			if ( e.PropertyName == Picker.TitleProperty.PropertyName )
			{
				Control.Text = Element.Title;
			}
			if ( e.PropertyName == Picker.SelectedIndexProperty.PropertyName && Element.SelectedIndex >= 0 && Element.SelectedIndex < Element.Items.Count )
			{
				Control.SelectedIndex = Element.SelectedIndex + 1;
			}
		}
		internal override void OnModelFocusChangeRequested(object sender, VisualElement.FocusRequestArgs args)
		{
			if (base.Control == null)
			{
				return;
			}
			if (args.Focus)
			{
				args.Result = this.OpenPickerPage();
				return;
			}
			args.Result = this.ClosePickerPage();
			base.UnfocusControl(base.Control);
		}
		private bool OpenPickerPage()
		{
			return Control.IsDropDownOpen = true;
		}

		bool ClosePickerPage()
		{
			Control.IsDropDownOpen = false;
			return true;
			/*FieldInfo field = typeof(ListPicker).GetField( "_listPickerPage", BindingFlags.Instance | BindingFlags.NonPublic );
			ListPickerPage target = field.GetValue( base.Control ) as ListPickerPage;
			typeof(ListPickerPage).InvokeMember( "ClosePickerPage", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, Type.DefaultBinder, target, null );
			return true;*/
		}
	}
}
