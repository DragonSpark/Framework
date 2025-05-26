using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components;

public class DropDownGrid<T> : RadzenDropDownDataGrid<T>, IRefreshAware
{
	[CascadingParameter]
	IRefreshContainer? Container
	{
		get;
		set
		{
			if (field != value)
			{
				field?.Remove.Execute(this);

				field = value;

				field?.Add.Execute(this);
			}
		}
	}

	protected override void OnParametersSet()
	{
		if (Container != null)
		{
			if (Visible && LoadData.HasDelegate && Data == null)
			{
				Visible = false;
				InvokeAsync(Reload);
			}
			else
			{
				Visible = Data != null;
			}
		}
		base.OnParametersSet();
	}

	public Task Get() => Reload();

	public override void Dispose()
	{
		Container = null;
		base.Dispose();
	}
}