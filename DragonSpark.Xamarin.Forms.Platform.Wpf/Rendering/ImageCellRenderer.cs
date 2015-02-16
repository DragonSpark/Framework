using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class ImageCellRenderer : ICellRenderer, IRegisterable
	{
		public virtual System.Windows.DataTemplate GetTemplate(Cell cell)
		{
			if (cell.Parent is ListView)
			{
				return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ListImageCell"];
			}
			return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["ImageCell"];
		}
	}
}
