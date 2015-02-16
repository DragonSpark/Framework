using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class TextCellRenderer : ICellRenderer, IRegisterable
	{
		public virtual System.Windows.DataTemplate GetTemplate(Cell cell)
		{
			if (!(cell.Parent is ListView))
			{
				return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["TextCell"];
			}
			if (TemplatedItemsList<ItemsView<Cell>, Cell>.GetIsGroupHeader(cell))
			{
				return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ListViewHeaderTextCell"];
			}
			return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ListViewTextCell"];
		}
	}
}
