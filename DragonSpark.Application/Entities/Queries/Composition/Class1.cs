using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	class Class1 {}

	public class Combine<T> : Combine<T, T>
	{
		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(previous, select) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
			: base(previous, select) {}
	}

	public class Combine<T, TTo> : Combine<None, T, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(Combine<T, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, _) => previous.Invoke(context), instance) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, _) => previous.Invoke(context), instance) {}
	}

	public class Combine<TIn, T, TTo> : InputQuery<TIn, TTo>
	{
		protected Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(context, previous.Invoke(context, @in))) {}

		public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		               Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(previous.Invoke(context, @in))) {}

		public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		               Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(@in, previous.Invoke(context, @in))) {}

		protected Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(context, @in, previous.Invoke(context, @in))) {}
	}

	sealed class Set<T> : Query<T> where T : class
	{
		public static Set<T> Default { get; } = new Set<T>();

		Set() : base(x => x.Set<T>()) {}
	}

	public class Set<TIn, T> : InputQuery<TIn, T> where T : class
	{
		public static Set<TIn, T> Default { get; } = new();

		Set() : base(x => x.Set<T>()) {}
	}

	public class Start<T> : Combine<T> where T : class
	{
		protected Start(Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(Set<T>.Default, select) {}

		protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
			: base(Set<T>.Default, select) {}
	}

	public class Start<T, TTo> : Combine<T, TTo> where T : class
	{
		protected Start(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select) : base(Set<T>.Default, select) {}

		protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: base(Set<T>.Default, select) {}
	}

	public class StartInput<TIn, T> : StartInput<TIn, T, T> where T : class
	{
		protected StartInput(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> instance)
			: base(instance) {}

		protected StartInput(Expression<Func<IQueryable<T>, IQueryable<T>>> instance) : base(instance) {}

		protected StartInput(Expression<Func<TIn, IQueryable<T>, IQueryable<T>>> instance) : base(instance) {}

		protected StartInput(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                     Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<T>>> instance)
			: base(previous, instance) {}
	}

	public class StartInput<TIn, T, TTo> : Combine<TIn, T, TTo> where T : class
	{
		protected StartInput(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
			: base(Set<TIn, T>.Default, instance) {}

		protected StartInput(Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
			: base(Set<TIn, T>.Default, instance) {}

		protected StartInput(Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base(Set<TIn, T>.Default, instance) {}

		protected StartInput(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                     Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base(previous, instance) {}
	}

/**/

	public class Where<T> : Where<None, T>, IQuery<T>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<T>>>(Where<T> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		public Where(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
			: base((context, _) => previous.Invoke(context), where) {}
	}

	public class Where<TIn, T> : Combine<TIn, T, T>
	{
		public Where(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
			: this(previous, (_, element) => where.Invoke(element)) {}

		public Where(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where)
			: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x))) {}
	}

	public class StartWhere<T> : Where<T> where T : class
	{
		protected StartWhere(Expression<Func<T, bool>> where) : base(Set<T>.Default, where) {}
	}

	public class StartWhere<TIn, T> : Where<TIn, T> where T : class
	{
		protected StartWhere(Expression<Func<T, bool>> where) : base(Set<TIn, T>.Default, where) {}

		public StartWhere(Expression<Func<TIn, T, bool>> where) : base(Set<TIn, T>.Default, where) {}
	}

	public class WhereSelect<T, TTo> : WhereSelect<None, T, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(WhereSelect<T, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		protected WhereSelect(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                      Expression<Func<T, TTo>> select)
			: base((context, _) => previous.Invoke(context), where, select) {}
	}

	public class WhereSelect<TIn, T, TTo> : Combine<TIn, T, TTo>
	{
		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                      Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: this(previous, (_, x) => where.Invoke(x), (_, x) => select.Invoke(x)) {}

		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                      Expression<Func<TIn, T, TTo>> select)
			: this(previous, (_, x) => where.Invoke(x), select) {}

		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                      Expression<Func<TIn, T, bool>> where, Expression<Func<T, TTo>> select)
			: this(previous, where, (_, x) => select.Invoke(x)) {}

		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                      Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, TTo>> select)
			: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x)).Select(x => select.Invoke(@in, x))) {}
	}

	public class StartWhereSelect<T, TTo> : WhereSelect<T, TTo> where T : class
	{
		protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: base(Set<T>.Default, where, select) {}
	}

	public class StartWhereSelect<TIn, T, TTo> : WhereSelect<TIn, T, TTo> where T : class
	{
		protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		protected StartWhereSelect(Expression<Func<T, bool>> @where, Expression<Func<TIn, T, TTo>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}

		protected StartWhereSelect(Expression<Func<TIn, T, bool>> @where, Expression<Func<T, TTo>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}

		protected StartWhereSelect(Expression<Func<TIn, T, bool>> @where, Expression<Func<TIn, T, TTo>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}
	}

	public class WhereMany<T, TTo> : WhereMany<None, T, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(WhereMany<T, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		public WhereMany(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                 Expression<Func<T, IEnumerable<TTo>>> select)
			: base((context, _) => previous.Invoke(context), where, select) {}
	}

	public class WhereMany<TIn, T, TTo> : Combine<TIn, T, TTo>
	{
		public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                 Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
			: this(previous, (_, x) => where.Invoke(x), (_, x) => select.Invoke(x)) {}

		public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                 Expression<Func<TIn, T, IEnumerable<TTo>>> select)
			: this(previous, (_, x) => where.Invoke(x), select) {}

		public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where,
		                 Expression<Func<T, IEnumerable<TTo>>> select)
			: this(previous, where, (_, x) => select.Invoke(x)) {}

		public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where,
		                 Expression<Func<TIn, T, IEnumerable<TTo>>> select)
			: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x)).SelectMany(x => select.Invoke(@in, x))) {}
	}

	public class StartWhereMany<T, TTo> : WhereMany<T, TTo> where T : class
	{
		protected StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
			: base(Set<T>.Default, where, select) {}
	}

	public class StartWhereMany<TIn, T, TTo> : WhereMany<TIn, T, TTo> where T : class
	{
		protected StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		public StartWhereMany(Expression<Func<T, bool>> @where, Expression<Func<TIn, T, IEnumerable<TTo>>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}

		public StartWhereMany(Expression<Func<TIn, T, bool>> @where, Expression<Func<T, IEnumerable<TTo>>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}

		public StartWhereMany(Expression<Func<TIn, T, bool>> @where, Expression<Func<TIn, T, IEnumerable<TTo>>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}
	}

	public class Select<TFrom, TTo> : Select<None, TFrom, TTo>, IQuery<TTo>
	{
		public Select(Expression<Func<DbContext, IQueryable<TFrom>>> previous, Expression<Func<TFrom, TTo>> select)
			: base((context, _) => previous.Invoke(context), select) {}
	}

	public class Select<TIn, TFrom, TTo> : Combine<TIn, TFrom, TTo>
	{
		public Select(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous, Expression<Func<TFrom, TTo>> select)
			: this(previous, (_, from) => select.Invoke(from)) {}

		public Select(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
		              Expression<Func<TIn, TFrom, TTo>> select)
			: base(previous, (@in, q) => q.Select(x => select.Invoke(@in, x))) {}
	}

	public class StartSelect<TFrom, TTo> : Select<TFrom, TTo> where TFrom : class
	{
		protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TFrom>.Default, select) {}
	}

	public class StartSelect<TIn, TFrom, TTo> : Select<TIn, TFrom, TTo> where TFrom : class
	{
		protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TIn, TFrom>.Default, select) {}
	}

	public class SelectMany<TFrom, TTo> : SelectMany<None, TFrom, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(SelectMany<TFrom, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		public SelectMany(Expression<Func<DbContext, IQueryable<TFrom>>> previous,
		                  Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base((context, _) => previous.Invoke(context), select) {}
	}

	public class SelectMany<TIn, TFrom, TTo> : Combine<TIn, TFrom, TTo>
	{
		public SelectMany(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
		                  Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: this(previous, (_, x) => select.Invoke(x)) {}

		public SelectMany(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
		                  Expression<Func<TIn, TFrom, IEnumerable<TTo>>> select)
			: base(previous, (@in, q) => q.SelectMany(x => select.Invoke(@in, x))) {}
	}

	public class StartSelectMany<TFrom, TTo> : SelectMany<TFrom, TTo> where TFrom : class
	{
		protected StartSelectMany(Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(Set<TFrom>.Default, select) {}
	}

	public class StartSelectMany<TIn, TFrom, TTo> : SelectMany<TIn, TFrom, TTo> where TFrom : class
	{
		protected StartSelectMany(Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(Set<TIn, TFrom>.Default, select) {}

		public StartSelectMany(Expression<Func<TIn, TFrom, IEnumerable<TTo>>> @select)
			: base(Set<TIn, TFrom>.Default, @select) {}
	}

/**/
	public class Introduce<TFrom, TOther, TTo> : Introduce<None, TFrom, TOther, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(
			Introduce<TFrom, TOther, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => other.Invoke(context),
			       (context, _, f, o) => select.Invoke(context, f, o)) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => other.Invoke(context),
			       (context, _, f, o) => select.Invoke(f, o)) {}
	}

	public class Introduce<TIn, TFrom, TOther, TTo> : InputQuery<TIn, TTo>
	{
		public Introduce(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, TIn, IQueryable<TOther>>> other,
		                 Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>>
			                 select)
			: base((context, @in)
				       => select.Invoke(context, @in, from.Invoke(context, @in), other.Invoke(context, @in))) {}

		public Introduce(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, TIn, IQueryable<TOther>>> other,
		                 Expression<Func<TIn, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base((context, @in) => select.Invoke(@in, from.Invoke(context, @in), other.Invoke(context, @in))) {}
	}

	public class StartIntroduce<TFrom, TOther, TTo> : Introduce<TFrom, TOther, TTo> where TFrom : class
	{
		protected StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
		                         Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, other, select) {}

		protected StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
		                         Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>>
			                         select)
			: base(Set<TFrom>.Default, other, select) {}
	}

	public class StartIntroduce<TIn, TFrom, TOther, TTo> : Introduce<TIn, TFrom, TOther, TTo> where TFrom : class
	{
		protected StartIntroduce(Expression<Func<DbContext, TIn, IQueryable<TOther>>> other,
		                         Expression<Func<TIn, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(Set<TIn, TFrom>.Default, other, select) {}

		protected StartIntroduce(Expression<Func<DbContext, TIn, IQueryable<TOther>>> other,
		                         Expression<
				                         Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>>
			                         select)
			: base(Set<TIn, TFrom>.Default, other, select) {}
	}

/**/

	public class IntroduceTwo<TFrom, T1, T2, TTo> : IntroduceTwo<None, TFrom, T1, T2, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(
			IntroduceTwo<TFrom, T1, T2, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		// ReSharper disable once TooManyDependencies
		public IntroduceTwo(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                    Expression<Func<DbContext, IQueryable<T1>>> first,
		                    Expression<Func<DbContext, IQueryable<T2>>> second,
		                    Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                    IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
			       (context, _) => second.Invoke(context),
			       (context, _, f, t1, t2) => select.Invoke(context, f, t1, t2)) {}

		// ReSharper disable once TooManyDependencies
		public IntroduceTwo(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                    Expression<Func<DbContext, IQueryable<T1>>> first,
		                    Expression<Func<DbContext, IQueryable<T2>>> second,
		                    Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
			       (context, _) => second.Invoke(context), (_, f, t1, t2) => select.Invoke(f, t1, t2)) {}
	}

	public class IntroduceTwo<TIn, TFrom, T1, T2, TTo> : InputQuery<TIn, TTo>
	{
		// ReSharper disable once TooManyDependencies
		public IntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                    Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                    Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                    Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                    IQueryable<TTo>>> select)
			: base((context, @in) => select.Invoke(context, @in, from.Invoke(context, @in), first.Invoke(context, @in),
			                                       second.Invoke(context, @in))) {}

		// ReSharper disable once TooManyDependencies
		public IntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                    Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                    Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                    Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>>
			                    select)
			: base((context, @in) => select.Invoke(@in, from.Invoke(context, @in), first.Invoke(context, @in),
			                                       second.Invoke(context, @in))) {}
	}

	public class StartIntroduceTwo<TFrom, T1, T2, TTo> : IntroduceTwo<TFrom, T1, T2, TTo> where TFrom : class
	{
		// ReSharper disable once TooManyDependencies
		protected StartIntroduceTwo(
			Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
			Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, first, second, select) {}

		// ReSharper disable once TooManyDependencies
		protected StartIntroduceTwo(
			Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
			Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, first, second, select) {}
	}

	public class StartIntroduceTwo<TIn, TFrom, T1, T2, TTo> : IntroduceTwo<TIn, TFrom, T1, T2, TTo> where TFrom : class
	{
		// ReSharper disable once TooManyDependencies
		protected StartIntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                            Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                            Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                            IQueryable<TTo>>> select)
			: base(Set<TIn, TFrom>.Default, first, second, select) {}

		// ReSharper disable once TooManyDependencies
		protected StartIntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                            Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                            Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                            IQueryable<TTo>>> select)
			: base(Set<TIn, TFrom>.Default, first, second, select) {}
	}

/**/

	public class IntroduceThree<TFrom, T1, T2, T3, TTo> : IntroduceThree<None, TFrom, T1, T2, T3, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(
			IntroduceThree<TFrom, T1, T2, T3, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		// ReSharper disable once TooManyDependencies
		public IntroduceThree(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                      Expression<Func<DbContext, IQueryable<T1>>> first,
		                      Expression<Func<DbContext, IQueryable<T2>>> second,
		                      Expression<Func<DbContext, IQueryable<T3>>> third,
		                      Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                      IQueryable<T3>, IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
			       (context, _) => second.Invoke(context),
			       (context, _) => third.Invoke(context),
			       (context, _, f, t1, t2, t3) => select.Invoke(context, f, t1, t2, t3)) {}

		// ReSharper disable once TooManyDependencies
		public IntroduceThree(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                      Expression<Func<DbContext, IQueryable<T1>>> first,
		                      Expression<Func<DbContext, IQueryable<T2>>> second,
		                      Expression<Func<DbContext, IQueryable<T3>>> third,
		                      Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                      IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
			       (context, _) => second.Invoke(context), (context, _) => third.Invoke(context),
			       (_, f, t1, t2, t3) => select.Invoke(f, t1, t2, t3)) {}
	}

	public class IntroduceThree<TIn, TFrom, T1, T2, T3, TTo> : InputQuery<TIn, TTo>
	{
		// ReSharper disable once TooManyDependencies
		public IntroduceThree(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                      Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
		                      Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                      IQueryable<T3>,
			                      IQueryable<TTo>>> select)
			: base((context, @in) => select.Invoke(context, @in, from.Invoke(context, @in), first.Invoke(context, @in),
			                                       second.Invoke(context, @in), third.Invoke(context, @in))) {}

		// ReSharper disable once TooManyDependencies
		public IntroduceThree(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                      Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
		                      Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                      IQueryable<TTo>>> select)
			: base((context, @in) => select.Invoke(@in, from.Invoke(context, @in), first.Invoke(context, @in),
			                                       second.Invoke(context, @in), third.Invoke(context, @in))) {}
	}

	public class StartIntroduceThree<TFrom, T1, T2, T3, TTo> : IntroduceThree<TFrom, T1, T2, T3, TTo>
		where TFrom : class
	{
		// ReSharper disable once TooManyDependencies
		protected StartIntroduceThree(
			Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
			Expression<Func<DbContext, IQueryable<T3>>> third,
			Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, first, second, third, select) {}

		// ReSharper disable once TooManyDependencies
		protected StartIntroduceThree(
			Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
			Expression<Func<DbContext, IQueryable<T3>>> third,
			Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
				IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, first, second, third, select) {}
	}

	public class StartIntroduceThree<TIn, TFrom, T1, T2, T3, TTo> : IntroduceThree<TIn, TFrom, T1, T2, T3, TTo>
		where TFrom : class
	{
		// ReSharper disable once TooManyDependencies
		protected StartIntroduceThree(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                              Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                              Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
		                              Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                              IQueryable<T3>, IQueryable<TTo>>> select)
			: base(Set<TIn, TFrom>.Default, first, second, third, select) {}

		// ReSharper disable once TooManyDependencies
		protected StartIntroduceThree(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                              Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                              Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
		                              Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                              IQueryable<T3>, IQueryable<TTo>>> select)
			: base(Set<TIn, TFrom>.Default, first, second, third, select) {}
	}
}