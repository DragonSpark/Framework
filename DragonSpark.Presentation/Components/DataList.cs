using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components;

public class DataList<T> : RadzenDataList<T>, IRefreshAware
{
	bool _update, _assigned;

	[Parameter]
	public int? PageIndex
	{
		get => CurrentPage;
		set
		{
			_assigned  = true;
			if (_pageIndex != value)
			{
				_update    = true;
				_pageIndex = value;
			}
			
			
		}
	} int? _pageIndex;

	[Parameter]
	public EventCallback<int?> PageIndexChanged { get; set; }

	[CascadingParameter]
	IRefreshContainer? Container
	{
		get => _container;
		set
		{
			if (_container != value)
			{
				_container?.Remove.Execute(this);

				_container = value;

				_container?.Add.Execute(this);
			}
		}
	}	IRefreshContainer? _container;

	public override async Task Reload()
	{
		if (_assigned)
		{
			if (_pageIndex is not null)
			{
				await PageIndexChanged.InvokeAsync((int)(_pageIndex = CurrentPage));	
			}
			else
			{
				return;
			}
		}
		await base.Reload().ConfigureAwait(false);
	}

	protected override async Task OnParametersSetAsync()
	{
		if (Visible && !_assigned && LoadData.HasDelegate && Data == null)
		{
			Visible = false;
			await base.Reload().ConfigureAwait(false);
		}
		else
		{
			if (_update && !(_update = false) && _pageIndex is not null)
			{
				await GoToPage(_pageIndex.Value, true).ConfigureAwait(false);
			}
			Visible |= Data != null;
		}
	}



	public Task Get() => Reload();

	public override void Dispose()
	{
		Container = null;
		base.Dispose();
	}
}