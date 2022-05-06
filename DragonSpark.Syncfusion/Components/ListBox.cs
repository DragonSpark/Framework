using Syncfusion.Blazor.DropDowns;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Components;

public class ListBox<TValue, TItem> : SfListBox<TValue, TItem>
{
	public Task Refresh()
	{
		ListData = DataSource.ToList();
		return RenderItems();
	}
}