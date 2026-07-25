using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class PagingContentContainer<T> : IPageContainer<T>
{
	QueryRenderState _state;
	Exception?       _error;
	IPages<T>?       _subject;

	[Parameter]
	public EventCallback<QueryRenderState> Changed { get; set; }

	[CascadingParameter] IPageContainer<T>? Parent { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Content), Content) ||
		              parameters.DidParameterChange(nameof(Compose), Compose);
		await base.SetParametersAsync(parameters).On();
		if (changed)
		{
			_subject = Paging.Get(new(this, Content.Verify(), Compose));
			_error   = null;
			_state   = QueryRenderState.Loading;
			await Changed.Off(_state);
		}
	}
	
	Task Update(bool? parameter)
	{
		_state = _error is not null
			         ? QueryRenderState.Error
			         : parameter is null
				         ? QueryRenderState.Loading
				         : parameter.Value
					         ? QueryRenderState.Ready
					         : QueryRenderState.Empty;
		if (Parent is null)
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