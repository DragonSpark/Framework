﻿using System;
using JetBrains.Annotations;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Invocation
{
	public class Deferred<TIn, TOut> : Select<TIn, TOut>, IActivateUsing<ISelect<TIn, TOut>>
	{
		[UsedImplicitly]
		public Deferred(ISelect<TIn, TOut> select)
			: this(select, Start.A.Selection<TIn>().AndOf<TOut>().Into.Table()) {}

		public Deferred(ISelect<TIn, TOut> select, ITable<TIn, TOut> assignable)
			: this(select, assignable, assignable) {}

		public Deferred(ISelect<TIn, TOut> select, IAssign<TIn, TOut> assign, ISelect<TIn, TOut> source)
			: base(new Configuration<TIn, TOut>(select, assign).Unless(source)) {}
	}

	public class Deferred<T> : Result<T>
	{
		public Deferred(Func<T> result, IMutable<T> mutable) : this(result.Start(), mutable) {}

		public Deferred(IResult<T> result, IMutable<T> mutable) : this(result, mutable, mutable) {}

		public Deferred(IResult<T> result, IResult<T> store, ICommand<T> assign)
			: base(result.Select(assign.Then().ToConfiguration().Get())
			             .Unless(store)) {}
	}
}