using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.History;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class PagingContentContainer<T> : IPageContainer<T>
{
	Variable<QueryRenderState> _state = new();
	Exception?                 _error;
	IPages<T>?                 _subject;

	[Parameter]
	public EventCallback<QueryRenderState> Changed { get; set; }

	[CascadingParameter] IPageContainer<T>? Parent { get; set; }
	[CascadingParameter] ContentScope? Scope { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Content), Content) ||
		              parameters.DidParameterChange(nameof(Compose), Compose);
		await base.SetParametersAsync(parameters).On();
		if (changed)
		{
			_subject = Paging.Get(new(this, Content.Verify(), Compose));
			_error   = null;
			if (Recent.Get(new(this, Scope?.Scope)) is PageStatus<T> entry)
			{
				await Get(entry).Off();
			}
			else
			{
				_state.Execute(QueryRenderState.Loading);
				await Changed.Off(_state);
			}
			
		}
	}
	
	Task Update(QueryRenderState? parameter)
	{
		var state = _error is not null ? QueryRenderState.Error : parameter ?? QueryRenderState.Loading;
		if (_state.Assign(state) && Parent is null)
		{
			if (Changed.HasDelegate)
			{
				return Changed.Invoke(_state);
			}
			StateHasChanged();
		}

		return Task.CompletedTask;
	}

	public Type Get() => ReportedType ?? GetType();

	public async ValueTask Get(PageStatus<T> parameter)
	{
		var (subject, filtered) = parameter;
		_error                  = null;
		Recent.Execute(new(this, Scope?.Scope), parameter);
		await Update(subject.Total is > 0 || filtered ? QueryRenderState.Ready : QueryRenderState.Empty).On();
		if (Parent is not null)
		{
			await Parent.Off(parameter);
		}
	}

	public async ValueTask Get(Exception parameter)
	{
		_error = parameter;
		Recent.Execute(new(this, Scope?.Scope), null);
		await Update(null).On();
		if (Parent is not null)
		{
			await Parent.Off(parameter);
		}
	}
}