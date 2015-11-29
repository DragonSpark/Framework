using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Xamarin.Forms;
using Label = Xamarin.Forms.Label;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class LabelRenderer : ViewRenderer<Label, TextBlock>
	{
		private class LabelTracker : VisualElementTracker<Label, FrameworkElement>
		{
			protected override void LayoutChild()
			{
				SizeRequest sizeRequest = base.Model.GetSizeRequest(base.Model.Width, double.PositiveInfinity);
				double num = Math.Max(0.0, Math.Min(base.Model.Height, sizeRequest.Request.Height));
				switch (base.Model.YAlign)
				{
				case global::Xamarin.Forms.TextAlignment.Start:
					Canvas.SetTop(this.Child, 0.0);
					goto IL_C2;
				case global::Xamarin.Forms.TextAlignment.End:
					Canvas.SetTop(this.Child, base.Model.Height - num);
					goto IL_C2;
				}
				Canvas.SetTop(this.Child, (double)((int)((base.Model.Height - num) / 2.0)));
				IL_C2:
				this.Child.Height = num;
				this.Child.Width = base.Model.Width;
			}
			protected override void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				base.HandlePropertyChanged(sender, e);
				if (e.PropertyName == Label.YAlignProperty.PropertyName)
				{
					this.UpdateNativeControl();
				}
			}
		}
		private bool fontApplied;
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			LabelRenderer.LabelTracker labelTracker = new LabelRenderer.LabelTracker();
			base.Tracker = labelTracker;
			base.OnElementChanged(e);
			TextBlock textBlock = new TextBlock
			{
				Foreground = (Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"]
			};
			this.UpdateText(textBlock);
			this.UpdateColor(textBlock);
			this.UpdateAlign(textBlock);
			this.UpdateFont(textBlock);
			this.UpdateLineBreakMode(textBlock);
			base.SetNativeControl(textBlock);
			labelTracker.Model = base.Element;
			labelTracker.Element = this;
		}
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (base.Control == null)
			{
				return default(SizeRequest);
			}
			System.Windows.Size availableSize = new System.Windows.Size(widthConstraint, heightConstraint);
			TextBlock control = base.Control;
			double width = control.Width;
			double height = control.Height;
			control.Height = ((heightConstraint == double.PositiveInfinity) ? double.NaN : heightConstraint);
			control.Width = ((widthConstraint == double.PositiveInfinity) ? double.NaN : widthConstraint);
			control.Measure(availableSize);
			global::Xamarin.Forms.Size size = new global::Xamarin.Forms.Size(Math.Ceiling(control.ActualWidth), Math.Ceiling(control.ActualHeight));
			control.Width = width;
			control.Height = height;
			global::Xamarin.Forms.Size minimum = size;
			minimum.Width = Math.Min(minimum.Width, 10.0);
			return new SizeRequest(size, minimum);
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Label.TextProperty.PropertyName || e.PropertyName == Label.FormattedTextProperty.PropertyName)
			{
				this.UpdateText(base.Control);
			}
			else
			{
				if (e.PropertyName == Label.TextColorProperty.PropertyName)
				{
					this.UpdateColor(base.Control);
				}
				else
				{
					if (e.PropertyName == Label.XAlignProperty.PropertyName)
					{
						this.UpdateAlign(base.Control);
					}
					else
					{
						if (e.PropertyName == Label.FontProperty.PropertyName)
						{
							this.UpdateFont(base.Control);
						}
						else
						{
							if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
							{
								this.UpdateLineBreakMode(base.Control);
							}
						}
					}
				}
			}
			base.OnElementPropertyChanged(sender, e);
		}
		private void UpdateLineBreakMode(TextBlock textBlock)
		{
			if (textBlock == null)
			{
				return;
			}
			switch (base.Element.LineBreakMode)
			{
			case LineBreakMode.NoWrap:
				textBlock.TextTrimming = TextTrimming.WordEllipsis;
				textBlock.TextWrapping = TextWrapping.NoWrap;
				return;
			case LineBreakMode.WordWrap:
				textBlock.TextTrimming = TextTrimming.None;
				textBlock.TextWrapping = TextWrapping.Wrap;
				return;
			case LineBreakMode.CharacterWrap:
				textBlock.TextTrimming = TextTrimming.WordEllipsis;
				textBlock.TextWrapping = TextWrapping.Wrap;
				return;
			case LineBreakMode.HeadTruncation:
				textBlock.TextTrimming = TextTrimming.WordEllipsis;
				textBlock.TextWrapping = TextWrapping.NoWrap;
				return;
			case LineBreakMode.TailTruncation:
				textBlock.TextTrimming = TextTrimming.WordEllipsis;
				textBlock.TextWrapping = TextWrapping.NoWrap;
				return;
			case LineBreakMode.MiddleTruncation:
				textBlock.TextTrimming = TextTrimming.WordEllipsis;
				textBlock.TextWrapping = TextWrapping.NoWrap;
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		private void UpdateFont(TextBlock textBlock)
		{
			if (textBlock == null)
			{
				return;
			}
			Label element = base.Element;
			if (element == null || (element.IsDefault() && !this.fontApplied))
			{
				return;
			}
			Font font = element.IsDefault() ? Font.SystemFontOfSize(NamedSize.Medium) : (Font)element.GetValue( Label.FontProperty );
			textBlock.ApplyFont(font);
			this.fontApplied = true;
		}
		private void UpdateAlign(TextBlock textBlock)
		{
			if (textBlock == null)
			{
				return;
			}
			Label element = base.Element;
			if (element == null)
			{
				return;
			}
			if (element.XAlign == global::Xamarin.Forms.TextAlignment.Start)
			{
				textBlock.TextAlignment = System.Windows.TextAlignment.Left;
				return;
			}
			if (element.XAlign == global::Xamarin.Forms.TextAlignment.End)
			{
				textBlock.TextAlignment = System.Windows.TextAlignment.Right;
				return;
			}
			textBlock.TextAlignment = System.Windows.TextAlignment.Center;
		}
		private void UpdateColor(TextBlock textBlock)
		{
			if (textBlock == null)
			{
				return;
			}
			Label element = base.Element;
			if (element != null && element.TextColor != global::Xamarin.Forms.Color.Default)
			{
				textBlock.Foreground = element.TextColor.ToBrush();
				return;
			}
			textBlock.Foreground = (Brush)System.Windows.Application.Current.Resources["PhoneForegroundBrush"];
		}
		private void UpdateText(TextBlock textBlock)
		{
			if (textBlock == null)
			{
				return;
			}
			Label element = base.Element;
			if (element != null)
			{
				if (element.FormattedText == null)
				{
					textBlock.Text = element.Text;
					return;
				}
				FormattedString formattedString = element.FormattedText ?? element.Text;
				textBlock.Inlines.Clear();
				foreach (Inline current in formattedString.ToInlines())
				{
					textBlock.Inlines.Add(current);
				}
			}
		}
	}
}
