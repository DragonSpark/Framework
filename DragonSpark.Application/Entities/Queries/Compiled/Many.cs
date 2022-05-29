using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Many<TIn, TOut> : IElements<TIn, TOut>
{
	readonly Func<DbContext, IAsyncEnumerable<TOut>> _select;

	public Many(Expression<Func<DbContext, IQueryable<TOut>>> expression) : this(EF.CompileAsyncQuery(expression)) {}

	public Many(Func<DbContext, IAsyncEnumerable<TOut>> select) => _select = select;

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter) => _select(parameter.Context);
}

sealed class Many<TIn, TOut, T1> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                               _first;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, IQueryable<TOut>>> expression, params Delegate[] delegates)
		: this(expression, delegates[0].To<Func<TIn, T1>>()) {}

	public Many(Expression<Func<DbContext, T1, IQueryable<TOut>>> expression, Func<TIn, T1> first)
		: this(EF.CompileAsyncQuery(expression), first) {}

	public Many(Func<DbContext, T1, IAsyncEnumerable<TOut>> select, Func<TIn, T1> first)
	{
		_select = select;
		_first  = first;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                   _first;
	readonly Func<TIn, T2>                                   _second;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, IQueryable<TOut>>> expression, params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>()) {}

	public Many(Expression<Func<DbContext, T1, T2, IQueryable<TOut>>> expression, Func<TIn, T1> first,
	            Func<TIn, T2> second)
		: this(EF.CompileAsyncQuery(expression), first, second) {}

	public Many(Func<DbContext, T1, T2, IAsyncEnumerable<TOut>> select, Func<TIn, T1> first,
	            Func<TIn, T2> second)
	{
		_select = select;
		_first  = first;
		_second = second;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2, T3> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                       _first;
	readonly Func<TIn, T2>                                       _second;
	readonly Func<TIn, T3>                                       _third;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, T3, IQueryable<TOut>>> expression,
	            params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>()) {}

	// ReSharper disable once TooManyDependencies
	public Many(Expression<Func<DbContext, T1, T2, T3, IQueryable<TOut>>> expression, Func<TIn, T1> first,
	            Func<TIn, T2> second, Func<TIn, T3> third)
		: this(EF.CompileAsyncQuery(expression), first, second, third) {}

	// ReSharper disable once TooManyDependencies
	public Many(Func<DbContext, T1, T2, T3, IAsyncEnumerable<TOut>> select, Func<TIn, T1> first,
	            Func<TIn, T2> second, Func<TIn, T3> third)
	{
		_select = select;
		_first  = first;
		_second = second;
		_third  = third;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2, T3, T4> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                           _first;
	readonly Func<TIn, T2>                                           _second;
	readonly Func<TIn, T3>                                           _third;
	readonly Func<TIn, T4>                                           _fourth;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, IQueryable<TOut>>> expression,
	            params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>()) {}

	// ReSharper disable once TooManyDependencies
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, IQueryable<TOut>>> expression, Func<TIn, T1> first,
	            Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth) {}

	// ReSharper disable once TooManyDependencies
	public Many(Func<DbContext, T1, T2, T3, T4, IAsyncEnumerable<TOut>> select, Func<TIn, T1> first,
	            Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth)
	{
		_select = select;
		_first  = first;
		_second = second;
		_third  = third;
		_fourth = fourth;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2, T3, T4, T5> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                               _first;
	readonly Func<TIn, T2>                                               _second;
	readonly Func<TIn, T3>                                               _third;
	readonly Func<TIn, T4>                                               _fourth;
	readonly Func<TIn, T5>                                               _fifth;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, IQueryable<TOut>>> expression,
	            params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>(),
		       delegates[4].To<Func<TIn, T5>>()) {}

	// ReSharper disable once TooManyDependencies
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, IQueryable<TOut>>> expression,
	            Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	            Func<TIn, T5> fifth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth) {}

	// ReSharper disable once TooManyDependencies
	public Many(Func<DbContext, T1, T2, T3, T4, T5, IAsyncEnumerable<TOut>> select, Func<TIn, T1> first,
	            Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth, Func<TIn, T5> fifth)
	{
		_select = select;
		_first  = first;
		_second = second;
		_third  = third;
		_fourth = fourth;
		_fifth  = fifth;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2, T3, T4, T5, T6> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                                   _first;
	readonly Func<TIn, T2>                                                   _second;
	readonly Func<TIn, T3>                                                   _third;
	readonly Func<TIn, T4>                                                   _fourth;
	readonly Func<TIn, T5>                                                   _fifth;
	readonly Func<TIn, T6>                                                   _sixth;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, IQueryable<TOut>>> expression,
	            params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>(),
		       delegates[4].To<Func<TIn, T5>>(),
		       delegates[5].To<Func<TIn, T6>>()) {}

	// ReSharper disable once TooManyDependencies
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, IQueryable<TOut>>> expression,
	            Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	            Func<TIn, T5> fifth, Func<TIn, T6> sixth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth) {}

	// ReSharper disable once TooManyDependencies
	public Many(Func<DbContext, T1, T2, T3, T4, T5, T6, IAsyncEnumerable<TOut>> select, Func<TIn, T1> first,
	            Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth, Func<TIn, T5> fifth,
	            Func<TIn, T6> sixth)
	{
		_select = select;
		_first  = first;
		_second = second;
		_third  = third;
		_fourth = fourth;
		_fifth  = fifth;
		_sixth  = sixth;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2, T3, T4, T5, T6, T7> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, T7, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                                       _first;
	readonly Func<TIn, T2>                                                       _second;
	readonly Func<TIn, T3>                                                       _third;
	readonly Func<TIn, T4>                                                       _fourth;
	readonly Func<TIn, T5>                                                       _fifth;
	readonly Func<TIn, T6>                                                       _sixth;
	readonly Func<TIn, T7>                                                       _seventh;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, IQueryable<TOut>>> expression,
	            params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>(),
		       delegates[4].To<Func<TIn, T5>>(),
		       delegates[5].To<Func<TIn, T6>>(),
		       delegates[6].To<Func<TIn, T7>>()) {}

	// ReSharper disable once TooManyDependencies
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, IQueryable<TOut>>> expression,
	            Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	            Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth, seventh) {}

	// ReSharper disable once TooManyDependencies
	public Many(Func<DbContext, T1, T2, T3, T4, T5, T6, T7, IAsyncEnumerable<TOut>> select, Func<TIn, T1> first,
	            Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth, Func<TIn, T5> fifth,
	            Func<TIn, T6> sixth, Func<TIn, T7> seventh)
	{
		_select  = select;
		_first   = first;
		_second  = second;
		_third   = third;
		_fourth  = fourth;
		_fifth   = fifth;
		_sixth   = sixth;
		_seventh = seventh;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in), _seventh(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2, T3, T4, T5, T6, T7, T8> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                                           _first;
	readonly Func<TIn, T2>                                                           _second;
	readonly Func<TIn, T3>                                                           _third;
	readonly Func<TIn, T4>                                                           _fourth;
	readonly Func<TIn, T5>                                                           _fifth;
	readonly Func<TIn, T6>                                                           _sixth;
	readonly Func<TIn, T7>                                                           _seventh;
	readonly Func<TIn, T8>                                                           _eighth;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, IQueryable<TOut>>> expression,
	            params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>(),
		       delegates[4].To<Func<TIn, T5>>(),
		       delegates[5].To<Func<TIn, T6>>(),
		       delegates[6].To<Func<TIn, T7>>(),
		       delegates[7].To<Func<TIn, T8>>()) {}

	// ReSharper disable once TooManyDependencies
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, IQueryable<TOut>>> expression,
	            Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	            Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh, Func<TIn, T8> eighth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth, seventh, eighth) {}

	// ReSharper disable once TooManyDependencies
	public Many(Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, IAsyncEnumerable<TOut>> select,
	            Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	            Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh, Func<TIn, T8> eighth)
	{
		_select  = select;
		_first   = first;
		_second  = second;
		_third   = third;
		_fourth  = fourth;
		_fifth   = fifth;
		_sixth   = sixth;
		_seventh = seventh;
		_eighth  = eighth;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in), _seventh(@in), _eighth(@in));
		return result;
	}
}

sealed class Many<TIn, TOut, T1, T2, T3, T4, T5, T6, T7, T8, T9> : IElements<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncEnumerable<TOut>> _select;
	readonly Func<TIn, T1>                                                               _first;
	readonly Func<TIn, T2>                                                               _second;
	readonly Func<TIn, T3>                                                               _third;
	readonly Func<TIn, T4>                                                               _fourth;
	readonly Func<TIn, T5>                                                               _fifth;
	readonly Func<TIn, T6>                                                               _sixth;
	readonly Func<TIn, T7>                                                               _seventh;
	readonly Func<TIn, T8>                                                               _eighth;
	readonly Func<TIn, T9>                                                               _ninth;

	[ActivatorUtilitiesConstructor]
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, IQueryable<TOut>>> expression,
	            params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>(),
		       delegates[4].To<Func<TIn, T5>>(),
		       delegates[5].To<Func<TIn, T6>>(),
		       delegates[6].To<Func<TIn, T7>>(),
		       delegates[7].To<Func<TIn, T8>>(),
		       delegates[8].To<Func<TIn, T9>>()) {}

	// ReSharper disable once TooManyDependencies
	public Many(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, IQueryable<TOut>>> expression,
	            Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	            Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh, Func<TIn, T8> eighth,
	            Func<TIn, T9> ninth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth, seventh, eighth,
		       ninth) {}

	// ReSharper disable once TooManyDependencies
	public Many(Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, IAsyncEnumerable<TOut>> select,
	            Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	            Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh, Func<TIn, T8> eighth,
	            Func<TIn, T9> ninth)
	{
		_select  = select;
		_first   = first;
		_second  = second;
		_third   = third;
		_fourth  = fourth;
		_fifth   = fifth;
		_sixth   = sixth;
		_seventh = seventh;
		_eighth  = eighth;
		_ninth   = ninth;
	}

	public IAsyncEnumerable<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in), _seventh(@in), _eighth(@in), _ninth(@in));
		return result;
	}
}