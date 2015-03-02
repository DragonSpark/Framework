using System;
using System.Windows;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	[Obsolete("Deprecated in favor of CellControl")]
	public class CellTemplateSelector : DataTemplateSelector
	{
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(CellTemplateSelector), new PropertyMetadata(delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((CellTemplateSelector)o).SetSource(e.OldValue, e.NewValue);
		}));
		public CellTemplateSelector()
		{
			base.Loaded += delegate(object sender, RoutedEventArgs args)
			{
				base.SetBinding(CellTemplateSelector.SourceProperty, new System.Windows.Data.Binding());
			};
			base.Unloaded += delegate(object sender, RoutedEventArgs args)
			{
				Cell cell = base.DataContext as Cell;
				if (cell != null)
				{
					cell.SendDisappearing();
				}
			};
		}
		public override System.Windows.DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			Cell cell = item as Cell;
			if (cell == null)
			{
				return null;
			}
			ICellRenderer handler = Registrar.Registered.GetHandler<ICellRenderer>(cell.GetType());
			return handler.GetTemplate(cell);
		}
		private void SetSource(object oldSource, object newSource)
		{
			Cell cell = oldSource as Cell;
			Cell cell2 = newSource as Cell;
			if (cell != null)
			{
				cell.SendDisappearing();
			}
			if (cell2 != null)
			{
				cell2.SendAppearing();
			}
		}
	}
}
