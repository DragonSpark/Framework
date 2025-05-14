using DragonSpark.Application.AspNet.Entities.Queries.Runtime;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class QueryContentContainer<T> : IPageContainer<T>
{
	IPages<T>?        _subject;
	IPageContainer<T> _relay = null!;

	[Parameter]
	public IQueries<T>? Content { get; set; }

	[Parameter]
	public ICompose<T> Compose { get; set; } = DefaultCompose<T>.Default;

	[Parameter]
	public IPagination<T>? Pagination { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var changed = parameters.DidParameterChange(nameof(Content), Content) ||
		              parameters.DidParameterChange(nameof(Compose), Compose);
		await base.SetParametersAsync(parameters).Off();
		if (changed)
		{
			_subject = DetermineSubject();
		}
	}

	IPages<T> DetermineSubject()
	{
		var content = Content.Verify();
		var pages   = Paging.Get(new(this, content, Compose));
		var result  = Pagination?.Get(pages) ?? pages;
		return result;
	}

	public void Execute(Page<T> parameter)
	{
		_relay.Execute(parameter);
	}

	public void Execute(Exception parameter)
	{
		_relay.Execute(parameter);
	}

	public Type Get() => _relay.Account()?.Get() ?? ReportedType ?? GetType();
}