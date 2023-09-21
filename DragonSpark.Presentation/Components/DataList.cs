﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components;

public class DataList<T> : RadzenDataList<T>, IRefreshAware
{
	Switch? _ready;
	Switch  _update = new (true);

	IgnoreEntryOperation _reload = null!;

	protected override void OnInitialized()
	{
		_reload = new IgnoreEntryOperation(new Operation(ReloadBody), TimeSpan.FromSeconds(1));
		base.OnInitialized();
	}

	[Parameter]
	public int PageIndex
	{
		get => CurrentPage;
		set => _pageIndex = _update ? value : _pageIndex;
	}	int _pageIndex;

	[Parameter]
	public EventCallback<int> PageIndexChanged { get; set; }

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

	public override Task Reload() => _reload.Get().AsTask();

	async ValueTask ReloadBody()
	{
		if ((_ready ?? false) && _pageIndex != CurrentPage)
		{
			await PageIndexChanged.InvokeAsync(_pageIndex = CurrentPage);
		}

		await base.Reload().ConfigureAwait(false);
	}

	protected override async Task OnParametersSetAsync()
	{
		if (LoadData.HasDelegate && (_ready?.Up() ?? false))
		{
			var index = _pageIndex;
			await OnPageChanged(new() { PageIndex = index, Skip = PageSize * index }).ConfigureAwait(false);
			_update.Up();
		}
	}

	[Parameter]
	public object Tag
	{
		get => _tag;
		set
		{
			if (_tag != value)
			{
				_tag   = value;
				_ready = new();

				if (_pageIndex != 0)
				{
					_update.Down();
					_pageIndex = 0;
				}
				_reload?.Execute();
			}
		}
	}	object _tag = default!;

	public Task Get() => Reload();

	public override void Dispose()
	{
		Container = null;
		base.Dispose();
	}
}