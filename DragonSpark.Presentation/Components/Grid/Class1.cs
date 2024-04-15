using DragonSpark.Model.Operations.Selection;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Grid;

class Class1;

public sealed class GridItemProvider<T> : ISelecting<GridItemsProviderRequest<T>, GridItemsProviderResult<T>>
{


	public ValueTask<GridItemsProviderResult<T>> Get(GridItemsProviderRequest<T> parameter)
	{
		//parameter.

		return default;
	}
}