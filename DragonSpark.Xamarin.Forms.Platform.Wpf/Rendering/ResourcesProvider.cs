using System.Windows.Controls;
using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	internal class ResourcesProvider : ISystemResourcesProvider
	{
		private global::Xamarin.Forms.ResourceDictionary dictionary;
		public IResourceDictionary GetSystemResources()
		{
			this.dictionary = new global::Xamarin.Forms.ResourceDictionary();
			this.UpdateStyles();
			return this.dictionary;
		}
		private void UpdateStyles()
		{
			TextBlock hackbox = new TextBlock();
			this.dictionary[Device.Styles.TitleStyleKey] = this.GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["PhoneTextTitle1Style"], hackbox);
			this.dictionary[Device.Styles.SubtitleStyleKey] = this.GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["PhoneTextTitle2Style"], hackbox);
			this.dictionary[Device.Styles.BodyStyleKey] = this.GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["PhoneTextNormalStyle"], hackbox);
			this.dictionary[Device.Styles.CaptionStyleKey] = this.GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["PhoneTextSmallStyle"], hackbox);
			this.dictionary[Device.Styles.ListItemTextStyleKey] = this.GetListItemTextStyle();
			this.dictionary[Device.Styles.ListItemDetailTextStyleKey] = this.GetListItemDetailTextStyle();
		}
		private global::Xamarin.Forms.Style GetListItemTextStyle()
		{
			return new global::Xamarin.Forms.Style(typeof(Label))
			{
				Setters = 
				{
					new global::Xamarin.Forms.Setter
					{
						Property = Label.FontSizeProperty,
						Value = 48
					}
				}
			};
		}
		private global::Xamarin.Forms.Style GetListItemDetailTextStyle()
		{
			return new global::Xamarin.Forms.Style(typeof(Label))
			{
				Setters = 
				{
					new global::Xamarin.Forms.Setter
					{
						Property = Label.FontSizeProperty,
						Value = 32
					}
				}
			};
		}
		private global::Xamarin.Forms.Style GetStyle(System.Windows.Style style, TextBlock hackbox)
		{
			hackbox.Style = style;
			return new global::Xamarin.Forms.Style(typeof(Label))
			{
				Setters = 
				{
					new global::Xamarin.Forms.Setter
					{
						Property = Label.FontFamilyProperty,
						Value = hackbox.FontFamily
					},
					new global::Xamarin.Forms.Setter
					{
						Property = Label.FontSizeProperty,
						Value = hackbox.FontSize
					}
				}
			};
		}
	}
}
