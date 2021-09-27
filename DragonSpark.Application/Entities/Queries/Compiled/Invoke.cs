﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled
{
	public readonly struct Invoke<T> : IDisposable
	{
		readonly IDisposable _disposable;

		public Invoke(DbContext context, IDisposable disposable, IAsyncEnumerable<T> elements)
		{
			_disposable = disposable;
			Context     = context;
			Elements    = elements;
		}

		public DbContext Context { get; }

		public IAsyncEnumerable<T> Elements { get; }

		public void Deconstruct(out DbContext context, out IDisposable disposable, out IAsyncEnumerable<T> elements)
		{
			context    = Context;
			disposable = _disposable;
			elements   = Elements;
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}


	public class Invoke<TIn, T> : IInvoke<TIn, T>
	{
		readonly IScopes  _invocations;
		readonly IForm<TIn, T> _form;

		public Invoke(IScopes invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(invocations, new Form<TIn, T>(expression)) {}

		public Invoke(IScopes invocations, IForm<TIn, T> form)
		{
			_invocations = invocations;
			_form        = form;
		}

		public async ValueTask<Invoke<T>> Get(TIn parameter)
		{
			var (context, session) = _invocations.Get();
			var form = _form.Get(new(context, parameter));
			return new(context, await session.Get(), form);
		}
	}
}