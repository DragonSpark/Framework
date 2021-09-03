using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	public readonly struct Invocation<T> : IAsyncDisposable
	{
		public Invocation(DbContext context, IAsyncEnumerable<T> elements)
		{
			Context  = context;
			Elements = elements;
		}

		public DbContext Context { get; }

		public IAsyncEnumerable<T> Elements { get; }

		public ValueTask DisposeAsync() => Context.DisposeAsync();

		public void Deconstruct(out DbContext context, out IAsyncEnumerable<T> elements)
		{
			context  = Context;
			elements = Elements;
		}
	}
}