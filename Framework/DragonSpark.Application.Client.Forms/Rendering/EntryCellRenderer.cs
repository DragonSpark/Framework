using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class EntryCellRenderer : ICellRenderer, IRegisterable
	{
		public virtual System.Windows.DataTemplate GetTemplate(Cell cell)
		{
			return (System.Windows.DataTemplate)System.Windows.Application.Current.Resources["EntryCell"];
		}
	}
}
