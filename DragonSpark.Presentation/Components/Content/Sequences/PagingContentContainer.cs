using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class PagingContentContainer<T> : IPageContainer<T>
{
	readonly Switch _error = false, _any = false, _loading = true;
	bool?           _results;

	[Parameter]
	public IPages<T>? Content { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var update = parameters.DidParameterChange(nameof(Content), Content);
		await base.SetParametersAsync(parameters).On();
		if (update)
		{
			_results = null;
			Update();
		}
	}

	void Update()
	{
		_any.Execute(Results.Get(_results));
		_loading.Execute(!_error && _results is null);
	}

	void ICommand<Page<T>>.Execute(Page<T> parameter)
	{
		Update(_results ?? parameter.Total is > 0);
	}

	void ICommand<Exception>.Execute(Exception parameter)
	{
		Update(null);
	}

	void Update(bool? parameter)
	{
		_error.Execute(parameter.HasValue);
		_results = parameter;
		Update();
		StateHasChanged();
	}

	public Type Get() => ReportedType ?? GetType();
}