using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public static class FontExtensions
	{
		internal static bool IsDefault(this IFontElement self)
		{
			return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) && self.FontAttributes == FontAttributes.None;
		}
		public static void ApplyFont(this Control self, Font font)
		{
			if (font.UseNamedSize)
			{
				switch (font.NamedSize)
				{
				case NamedSize.Micro:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"] - 3.0;
					break;
				case NamedSize.Small:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"];
					break;
				case NamedSize.Medium:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeMedium"];
					break;
				case NamedSize.Large:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeLarge"];
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				self.FontSize = font.FontSize;
			}
			if (!string.IsNullOrEmpty(font.FontFamily))
			{
				self.FontFamily = new FontFamily(font.FontFamily);
			}
			else
			{
				self.FontFamily = (FontFamily)System.Windows.Application.Current.Resources["PhoneFontFamilySemiBold"];
			}
			if (font.FontAttributes.HasFlag(FontAttributes.Italic))
			{
				self.FontStyle = FontStyles.Italic;
			}
			else
			{
				self.FontStyle = FontStyles.Normal;
			}
			if (font.FontAttributes.HasFlag(FontAttributes.Bold))
			{
				self.FontWeight = FontWeights.Bold;
				return;
			}
			self.FontWeight = FontWeights.Normal;
		}
		public static void ApplyFont(this TextBlock self, Font font)
		{
			if (font.UseNamedSize)
			{
				switch (font.NamedSize)
				{
				case NamedSize.Micro:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"] - 3.0;
					break;
				case NamedSize.Small:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"];
					break;
				case NamedSize.Medium:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeNormal"];
					break;
				case NamedSize.Large:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeLarge"];
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				self.FontSize = font.FontSize;
			}
			if (!string.IsNullOrEmpty(font.FontFamily))
			{
				self.FontFamily = new FontFamily(font.FontFamily);
			}
			else
			{
				self.FontFamily = (FontFamily)System.Windows.Application.Current.Resources["PhoneFontFamilyNormal"];
			}
			if (font.FontAttributes.HasFlag(FontAttributes.Italic))
			{
				self.FontStyle = FontStyles.Italic;
			}
			else
			{
				self.FontStyle = FontStyles.Normal;
			}
			if (font.FontAttributes.HasFlag(FontAttributes.Bold))
			{
				self.FontWeight = FontWeights.Bold;
				return;
			}
			self.FontWeight = FontWeights.Normal;
		}
		public static void ApplyFont(this TextElement self, Font font)
		{
			if (font.UseNamedSize)
			{
				switch (font.NamedSize)
				{
				case NamedSize.Micro:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"] - 3.0;
					break;
				case NamedSize.Small:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeSmall"];
					break;
				case NamedSize.Medium:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeNormal"];
					break;
				case NamedSize.Large:
					self.FontSize = (double)System.Windows.Application.Current.Resources["PhoneFontSizeLarge"];
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				self.FontSize = font.FontSize;
			}
			if (!string.IsNullOrEmpty(font.FontFamily))
			{
				self.FontFamily = new FontFamily(font.FontFamily);
			}
			else
			{
				self.FontFamily = (FontFamily)System.Windows.Application.Current.Resources["PhoneFontFamilyNormal"];
			}
			if (font.FontAttributes.HasFlag(FontAttributes.Italic))
			{
				self.FontStyle = FontStyles.Italic;
			}
			else
			{
				self.FontStyle = FontStyles.Normal;
			}
			if (font.FontAttributes.HasFlag(FontAttributes.Bold))
			{
				self.FontWeight = FontWeights.Bold;
				return;
			}
			self.FontWeight = FontWeights.Normal;
		}
	}
}
