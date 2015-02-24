using System.Linq;
using DragonSpark.Extensions;
using Xamarin.Forms;
using DataTemplate = System.Windows.DataTemplate;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public interface ICellRenderer : IRegisterable
	{
		System.Windows.DataTemplate GetTemplate(Cell cell);
	}

	public class CellRenderer : ICellRenderer
	{
		public DataTemplate GetTemplate( Cell cell )
		{
			var type = cell.GetType();
			var result = new object[] { cell, type, type.Name }.Select( System.Windows.Application.Current.TryFindResource ).NotNull().FirstOrDefaultOfType<DataTemplate>();
			return result;
		}
	}
}
