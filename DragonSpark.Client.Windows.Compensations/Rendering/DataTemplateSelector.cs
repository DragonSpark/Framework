using System.Windows;
using System.Windows.Controls;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public abstract class DataTemplateSelector : ContentControl
	{
		public abstract System.Windows.DataTemplate SelectTemplate(object item, DependencyObject container);
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			base.ContentTemplate = this.SelectTemplate(newContent, this);
		}
	}
}
