using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Compile<TIn, TOut> : ISelect<Expression<Func<DbContext, TIn, TOut>>, IElement<TIn, TOut>>
{
	public static Compile<TIn, TOut> Default { get; } = new Compile<TIn, TOut>();

	Compile() : this(Candidates<TIn, TOut>.Default, A.Type<TIn>(), A.Type<TOut>()) {}

	readonly Array<Generic<TIn, TOut>> _generics;
	readonly Type[]                    _types;

	public Compile(Array<Generic<TIn, TOut>> generics, params Type[] types)
	{
		_generics = generics;
		_types    = types;
	}

	public IElement<TIn, TOut> Get(Expression<Func<DbContext, TIn, TOut>> parameter)
	{
		var (lambda, types, delegates) = new ParameterUsageEditor(parameter).Rewrite();
		switch (types.Length)
		{
			case 0:
				return new Compiled<TIn, TOut>((Expression<Func<DbContext, TOut>>)lambda);
			default:
				var all    = types.Open().Prepend(_types).ToArray();
				var result = _generics[types.Length - 1].Get(all)(lambda, delegates);
				return result;
		}
	}
}