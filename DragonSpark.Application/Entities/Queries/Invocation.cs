using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	public readonly struct Invocation<T>
	{
		readonly DbContext           _context;
		readonly ISession            _session;
		readonly IAsyncEnumerable<T> _elements;

		public Invocation(DbContext context, ISession session, IAsyncEnumerable<T> elements)
		{
			_context  = context;
			_session  = session;
			_elements = elements;
		}

		public async ValueTask<Invoke<T>> Invoke() => new(_context, await _session.Get(), _elements);
	}

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

		public void Deconstruct(out DbContext context, out IAsyncEnumerable<T> elements)
		{
			context  = Context;
			elements = Elements;
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}
	}
}