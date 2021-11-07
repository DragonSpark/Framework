using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Compiled<TIn, TOut> : IElement<TIn, TOut>
{
	readonly Func<DbContext, Task<TOut>> _select;

	public Compiled(Expression<Func<DbContext, TOut>> expression) : this(EF.CompileAsyncQuery(expression)) {}

	public Compiled(Func<DbContext, Task<TOut>> select) => _select = select;

	public Task<TOut> Get(In<TIn> parameter) => _select(parameter.Context);
}

sealed class Compiled<TIn, TOut, T1> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, Task<TOut>> _select;
	readonly Func<TIn, T1>                               _first;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, TOut>> expression, params Delegate[] delegates)
		: this(expression, delegates[0].To<Func<TIn, T1>>()) {}

	public Compiled(Expression<Func<DbContext, T1, TOut>> expression, Func<TIn, T1> first)
		: this(EF.CompileAsyncQuery(expression), first) {}

	public Compiled(Func<DbContext, T1, Task<TOut>> select, Func<TIn, T1> first)
	{
		_select = select;
		_first  = first;
	}

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, Task<TOut>> _select;
	readonly Func<TIn, T1>                                   _first;
	readonly Func<TIn, T2>                                   _second;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, T2, TOut>> expression, params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>()) {}

	public Compiled(Expression<Func<DbContext, T1, T2, TOut>> expression, Func<TIn, T1> first,
	                Func<TIn, T2> second)
		: this(EF.CompileAsyncQuery(expression), first, second) {}

	public Compiled(Func<DbContext, T1, T2, Task<TOut>> select, Func<TIn, T1> first,
	                Func<TIn, T2> second)
	{
		_select = select;
		_first  = first;
		_second = second;
	}

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2, T3> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, Task<TOut>> _select;
	readonly Func<TIn, T1>                                       _first;
	readonly Func<TIn, T2>                                       _second;
	readonly Func<TIn, T3>                                       _third;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, T2, T3, TOut>> expression,
	                params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>()) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Expression<Func<DbContext, T1, T2, T3, TOut>> expression, Func<TIn, T1> first,
	                Func<TIn, T2> second, Func<TIn, T3> third)
		: this(EF.CompileAsyncQuery(expression), first, second, third) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Func<DbContext, T1, T2, T3, Task<TOut>> select, Func<TIn, T1> first,
	                Func<TIn, T2> second, Func<TIn, T3> third)
	{
		_select = select;
		_first  = first;
		_second = second;
		_third  = third;
	}

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2, T3, T4> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, Task<TOut>> _select;
	readonly Func<TIn, T1>                                           _first;
	readonly Func<TIn, T2>                                           _second;
	readonly Func<TIn, T3>                                           _third;
	readonly Func<TIn, T4>                                           _fourth;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, TOut>> expression,
	                params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>()) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, TOut>> expression, Func<TIn, T1> first,
	                Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Func<DbContext, T1, T2, T3, T4, Task<TOut>> select, Func<TIn, T1> first,
	                Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth)
	{
		_select = select;
		_first  = first;
		_second = second;
		_third  = third;
		_fourth = fourth;
	}

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2, T3, T4, T5> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, Task<TOut>> _select;
	readonly Func<TIn, T1>                                               _first;
	readonly Func<TIn, T2>                                               _second;
	readonly Func<TIn, T3>                                               _third;
	readonly Func<TIn, T4>                                               _fourth;
	readonly Func<TIn, T5>                                               _fifth;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, TOut>> expression,
	                params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>(),
		       delegates[4].To<Func<TIn, T5>>()) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, TOut>> expression,
	                Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	                Func<TIn, T5> fifth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Func<DbContext, T1, T2, T3, T4, T5, Task<TOut>> select, Func<TIn, T1> first,
	                Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth, Func<TIn, T5> fifth)
	{
		_select = select;
		_first  = first;
		_second = second;
		_third  = third;
		_fourth = fourth;
		_fifth  = fifth;
	}

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2, T3, T4, T5, T6> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, Task<TOut>> _select;
	readonly Func<TIn, T1>                                                   _first;
	readonly Func<TIn, T2>                                                   _second;
	readonly Func<TIn, T3>                                                   _third;
	readonly Func<TIn, T4>                                                   _fourth;
	readonly Func<TIn, T5>                                                   _fifth;
	readonly Func<TIn, T6>                                                   _sixth;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, TOut>> expression,
	                params Delegate[] delegates)
		: this(expression,
		       delegates[0].To<Func<TIn, T1>>(),
		       delegates[1].To<Func<TIn, T2>>(),
		       delegates[2].To<Func<TIn, T3>>(),
		       delegates[3].To<Func<TIn, T4>>(),
		       delegates[4].To<Func<TIn, T5>>(),
		       delegates[5].To<Func<TIn, T6>>()) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, TOut>> expression,
	                Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	                Func<TIn, T5> fifth, Func<TIn, T6> sixth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Func<DbContext, T1, T2, T3, T4, T5, T6, Task<TOut>> select, Func<TIn, T1> first,
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

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2, T3, T4, T5, T6, T7> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, T7, Task<TOut>> _select;
	readonly Func<TIn, T1>                                                       _first;
	readonly Func<TIn, T2>                                                       _second;
	readonly Func<TIn, T3>                                                       _third;
	readonly Func<TIn, T4>                                                       _fourth;
	readonly Func<TIn, T5>                                                       _fifth;
	readonly Func<TIn, T6>                                                       _sixth;
	readonly Func<TIn, T7>                                                       _seventh;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, TOut>> expression,
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
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, TOut>> expression,
	                Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	                Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth, seventh) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Func<DbContext, T1, T2, T3, T4, T5, T6, T7, Task<TOut>> select, Func<TIn, T1> first,
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

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in), _seventh(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2, T3, T4, T5, T6, T7, T8> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, Task<TOut>> _select;
	readonly Func<TIn, T1>                                                           _first;
	readonly Func<TIn, T2>                                                           _second;
	readonly Func<TIn, T3>                                                           _third;
	readonly Func<TIn, T4>                                                           _fourth;
	readonly Func<TIn, T5>                                                           _fifth;
	readonly Func<TIn, T6>                                                           _sixth;
	readonly Func<TIn, T7>                                                           _seventh;
	readonly Func<TIn, T8>                                                           _eighth;

	[ActivatorUtilitiesConstructor]
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, TOut>> expression,
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
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, TOut>> expression,
	                Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	                Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh, Func<TIn, T8> eighth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth, seventh, eighth) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, Task<TOut>> select,
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

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in), _seventh(@in), _eighth(@in));
		return result;
	}
}

sealed class Compiled<TIn, TOut, T1, T2, T3, T4, T5, T6, T7, T8, T9> : IElement<TIn, TOut>
{
	readonly Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TOut>> _select;
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
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>> expression,
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
	public Compiled(Expression<Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>> expression,
	                Func<TIn, T1> first, Func<TIn, T2> second, Func<TIn, T3> third, Func<TIn, T4> fourth,
	                Func<TIn, T5> fifth, Func<TIn, T6> sixth, Func<TIn, T7> seventh, Func<TIn, T8> eighth,
	                Func<TIn, T9> ninth)
		: this(EF.CompileAsyncQuery(expression), first, second, third, fourth, fifth, sixth, seventh, eighth,
		       ninth) {}

	// ReSharper disable once TooManyDependencies
	public Compiled(Func<DbContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TOut>> select,
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

	public Task<TOut> Get(In<TIn> parameter)
	{
		var (context, @in) = parameter;
		var result = _select(context, _first(@in), _second(@in), _third(@in), _fourth(@in), _fifth(@in),
		                     _sixth(@in), _seventh(@in), _eighth(@in), _ninth(@in));
		return result;
	}
}