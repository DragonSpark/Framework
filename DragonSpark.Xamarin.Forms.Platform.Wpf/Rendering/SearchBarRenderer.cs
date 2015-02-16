using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class SearchBarRenderer : ViewRenderer<SearchBar, EntryTextBox>
	{
		private const string DefaultPlaceholder = "Search";
		protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
		{
			base.OnElementChanged(e);
			InputScope inputScope = new InputScope();
			InputScopeName inputScopeName = new InputScopeName { NameValue = InputScopeNameValue.Default };
			inputScope.Names.Add(inputScopeName);
			EntryTextBox entryTextBox = new EntryTextBox
			{
				Hint = base.Element.Placeholder ?? "Search",
				Text = base.Element.Text ?? "",
				InputScope = inputScope
			};
			entryTextBox.KeyUp += new KeyEventHandler(this.PhoneTextBoxOnKeyUp);
			entryTextBox.TextChanged += new TextChangedEventHandler(this.PhoneTextBoxOnTextChanged);
			base.SetNativeControl(entryTextBox);
		}
		private void PhoneTextBoxOnKeyUp(object sender, KeyEventArgs keyEventArgs)
		{
			if (keyEventArgs.Key == Key.Enter)
			{
				base.Element.OnSearchButtonPressed();
			}
		}
		private void PhoneTextBoxOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs textChangedEventArgs)
		{
			((IElementController)base.Element).SetValueFromRenderer(SearchBar.TextProperty, base.Control.Text);
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			string propertyName;
			if ((propertyName = e.PropertyName) != null)
			{
				if (propertyName == "Placeholder")
				{
					base.Control.Hint = (base.Element.Placeholder ?? "Search");
					return;
				}
				if (!(propertyName == "Text"))
				{
					return;
				}
				base.Control.Text = (base.Element.Text ?? "");
			}
		}
		protected override void UpdateBackgroundColor()
		{
			base.Control.Background = ((base.Element.BackgroundColor == global::Xamarin.Forms.Color.Default) ? ((Brush)System.Windows.Application.Current.Resources["PhoneTextBoxBrush"]) : base.Element.BackgroundColor.ToBrush());
		}
	}
}
