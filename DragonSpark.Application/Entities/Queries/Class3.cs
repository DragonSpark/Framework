using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class3 {}

	public readonly record struct In<T>(DbContext Context, T Parameter);

	public interface IForm<TIn, out T> : ISelect<In<TIn>, IAsyncEnumerable<T>> {}

	public readonly struct Invocation<T> : IAsyncDisposable, IDisposable
	{
		readonly IAsyncDisposable _disposable;

		public Invocation(IAsyncDisposable disposable, IAsyncEnumerable<T> elements)
		{
			_disposable = disposable;
			Elements    = elements;
		}

		public IAsyncEnumerable<T> Elements { get; }

		public ValueTask DisposeAsync() => _disposable.DisposeAsync();

		public void Dispose() {}
	}

	public interface IInvoke<in TIn, T> : ISelect<TIn, Invocation<T>> {}

}