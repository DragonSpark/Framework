using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class ManyCompile<TIn, TOut>
	: ISelect<Expression<Func<DbContext, TIn, IQueryable<TOut>>>, IElements<TIn, TOut>>
{
	public static ManyCompile<TIn, TOut> Default { get; } = new ManyCompile<TIn, TOut>();

	ManyCompile() : this(ManyCandidates<TIn, TOut>.Default, A.Type<TIn>(), A.Type<TOut>()) {}

	readonly Array<ManyGeneric<TIn, TOut>> _generics;
	readonly Type[]                    _types;

	public ManyCompile(Array<ManyGeneric<TIn, TOut>> generics, params Type[] types)
	{
		_generics = generics;
		_types    = types;
	}

	public IElements<TIn, TOut> Get(Expression<Func<DbContext, TIn, IQueryable<TOut>>> parameter)
	{
		var (lambda, types, delegates) = new ParameterUsageEditor(parameter).Rewrite();
		switch (types.Length)
		{
			case 0:
				return new Many<TIn, TOut>((Expression<Func<DbContext, IQueryable<TOut>>>)lambda);
			default:
				var all    = types.Open().Prepend(_types).ToArray();
				var result = _generics[types.Length - 1].Get(all)(lambda, delegates);
				return result;
		}
	}
}