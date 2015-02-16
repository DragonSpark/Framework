using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class EditorRenderer : ViewRenderer<Editor, System.Windows.Controls.TextBox>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox
			{
				VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
				TextWrapping = TextWrapping.Wrap,
				AcceptsReturn = true
			};
			base.SetNativeControl(textBox);
			this.UpdateText();
			this.UpdateInputScope();
			base.Control.LostFocus += delegate(object sender, RoutedEventArgs args)
			{
				base.Element.SendCompleted();
			};
			textBox.TextChanged += new TextChangedEventHandler(this.TextBoxOnTextChanged);
		}
		private void TextBoxOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs textChangedEventArgs)
		{
			((IElementController)base.Element).SetValueFromRenderer(Editor.TextProperty, base.Control.Text);
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Editor.TextProperty.PropertyName)
			{
				this.UpdateText();
				return;
			}
			if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
			{
				this.UpdateInputScope();
			}
		}
		private void UpdateText()
		{
			base.Control.Text = (base.Element.Text ?? "");
		}
		private void UpdateInputScope()
		{
			base.Control.InputScope = base.Element.Keyboard.ToInputScope();
		}
	}
}
