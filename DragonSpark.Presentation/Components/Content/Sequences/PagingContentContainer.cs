using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class PagingContentContainer<T> : IPageContainer<T>
{
	readonly Switch _any = false, _loading = true;
	Exception?      _error;
	bool?           _results;

	[Parameter]
	public IPages<T>? Content { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var update = parameters.DidParameterChange(nameof(Content), Content);
		await base.SetParametersAsync(parameters).On();
		if (update)
		{
			_error   = null;
			_results = null;
			Update();
		}
	}

	void Update()
	{
		_loading.Execute(_error is null && _results is null);
		_any.Execute(!_loading && (_results ?? false));
	}

	void ICommand<PageResult<T>>.Execute(PageResult<T> parameter)
	{
		_error = null;
		Update(parameter.Total is > 0);
	}

	void ICommand<Exception>.Execute(Exception parameter)
	{
		_error = parameter;
		Update(null);
	}

	void Update(bool? parameter)
	{
		_results = parameter;
		Update();
		StateHasChanged();
	}

	public Type Get() => ReportedType ?? GetType();
}