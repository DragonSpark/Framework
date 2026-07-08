using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
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
	IPages<T>?      _subject;

	[CascadingParameter] IPageContainer<T>? Parent { get; set; }

	[Parameter, EditorRequired]
	public required Switch Ready { get; set; }

	[Parameter]
	public EventCallback Updated { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Content), Content) ||
		              parameters.DidParameterChange(nameof(Compose), Compose);
		await base.SetParametersAsync(parameters).Off();
		if (changed)
		{
			Ready.Down();
			_subject = DetermineSubject();
			_error   = null;
			_results = null;
			Update();
		}
	}

	IPages<T> DetermineSubject()
	{
		var content = Content.Verify();
		var result  = Paging.Get(new(this, content, Compose));
		return result;
	}

	void Update()
	{
		_loading.Execute(_error is null && _results is null);
		_any.Execute(!_loading && (_results ?? false));
	}

	Task Update(bool? parameter)
	{
		_results = parameter;
		Update();
		Ready.Up();
		if (Parent is null)
		{
			if (Updated.HasDelegate)
			{
				return Updated.Invoke();
			}
			StateHasChanged();
		}

		return Task.CompletedTask;
	}

	public Type Get() => ReportedType ?? GetType();

	public async ValueTask Get(PageResult<T> parameter)
	{
		_error = null;
		await Update(parameter.Total is > 0).On();
		if (Parent is not null)
		{
			await Parent.Off(parameter);
		}
	}

	public async ValueTask Get(Exception parameter)
	{
		_error = parameter;
		await Update(null).On();
		if (Parent is not null)
		{
			await Parent.Off(parameter);
		}
	}
}