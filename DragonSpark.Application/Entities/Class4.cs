﻿using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities
{
	class Class4 {}

	public readonly record struct In<T>(DbContext Context, T Parameter);

	public interface IFormed<T> : IOperation<In<T>> {}

	public class Formed<T> : Operation<In<T>>, IFormed<T>
	{
		public Formed(ISelect<In<T>, ValueTask> @select) : base(@select) {}

		public Formed(Func<In<T>, ValueTask> @select) : base(@select) {}
	}

	public class AddFormed<T, TTo> : IFormed<T> where TTo : class
	{
		readonly IForming<T, TTo> _select;
		readonly IModify<TTo>     _add;

		protected AddFormed(ISelecting<T, TTo> selecting) : this(new Adapter<T, TTo>(selecting)) {}

		protected AddFormed(IForming<T, TTo> select) : this(@select, Update<TTo>.Default) {}

		protected AddFormed(IForming<T, TTo> select, IModify<TTo> add)
		{
			_select = @select;
			_add    = add;
		}

		public async ValueTask Get(In<T> parameter)
		{
			var entity = await _select.Await(parameter);
			_add.Execute(parameter.Subject(entity));
		}
	}

	public class Adding<T, TTo> : IForming<T, TTo> where TTo : class
	{
		readonly IForming<T, TTo> _select;
		readonly IModify<TTo>     _add;

		protected Adding(ISelecting<T, TTo> selecting) : this(new Adapter<T, TTo>(selecting)) {}

		protected Adding(IForming<T, TTo> select) : this(@select, Update<TTo>.Default) {}

		protected Adding(IForming<T, TTo> select, IModify<TTo> add)
		{
			_select = @select;
			_add    = add;
		}

		public async ValueTask<TTo> Get(In<T> parameter)
		{
			var result = await _select.Await(parameter);
			_add.Execute(parameter.Subject(result));
			return result;
		}
	}

	sealed class Adapter<TIn, TOut> : IForming<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut> _selecting;

		public Adapter(ISelecting<TIn, TOut> selecting) => _selecting = selecting;

		public ValueTask<TOut> Get(In<TIn> parameter) => _selecting.Get(parameter.Parameter);
	}

	public interface IForming<T> : IForming<None, T> {}

	public interface IForming<TIn, T> : ISelecting<In<TIn>, T> {}

	public class Forming<T> : Forming<None, T>, IForming<T>
	{
		public Forming(ISelect<In<None>, ValueTask<T>> @select) : base(@select) {}

		public Forming(Func<In<None>, ValueTask<T>> @select) : base(@select) {}
	}

	public class Forming<TIn, T> : Selecting<In<TIn>, T>, IForming<TIn, T>
	{
		public Forming(ISelect<In<TIn>, ValueTask<T>> select) : base(select) {}

		protected Forming(Func<In<TIn>, ValueTask<T>> select) : base(select) {}
	}

	sealed class Forming<TIn, T, TOut> : Selecting<In<TIn>, IAsyncEnumerable<T>, TOut>, IForming<TIn, TOut>
	{
		public Forming(IForm<TIn, T> form, IEvaluate<T, TOut> evaluate) : base(form.Then().Operation(), evaluate.Get) {}
	}
}