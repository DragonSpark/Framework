﻿using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class EditingOrNew<TIn, T> : IEdit<TIn, T> where T : class
{
	readonly IEdit<TIn, T?>     _previous;
	readonly ISelecting<TIn, T> _create;

	protected EditingOrNew(IScopes context, ISelecting<TIn, T> create, IQuery<TIn, T> query)
		: this(new Edit(context, query), create) {}

	protected EditingOrNew(IEdit<TIn, T?> previous, ISelecting<TIn, T> create)
	{
		_previous = previous;
		_create   = create;
	}

	public async ValueTask<Edit<T>> Get(TIn parameter)
	{
		var (context, current) = await _previous.Await(parameter);
		if (current is null)
		{
			var subject = await _create.Await(parameter);
			context.Update(subject);
			return new(context, subject);
		}
		return new(context, current);
	}

	sealed class Edit : EditingOrDefault<TIn, T>
	{
		public Edit(IScopes context, IQuery<TIn, T> query) : base(context, query) {}
	}
}