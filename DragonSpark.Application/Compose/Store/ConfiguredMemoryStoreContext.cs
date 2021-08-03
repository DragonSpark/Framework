using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store
{
	public sealed class ConfiguredMemoryStoreContext<TIn, TOut> : MemoryStoreContext<TIn, TOut>
	{
		readonly ICommand<ICacheEntry> _configure;

		public ConfiguredMemoryStoreContext(ISelect<TIn, TOut> subject, IMemoryCache memory,
		                                    ICommand<ICacheEntry> configure)
			: base(subject, memory)
			=> _configure = configure;

		public Selector<TIn, TOut> Using<T>() => Using(A.Type<T>().AssemblyQualifiedName.Verify().Accept);

		public Selector<TIn, TOut> Using<T>(Func<TIn, string> key)
			=> Using(new Key<TIn>(A.Type<T>().AssemblyQualifiedName ?? throw new InvalidOperationException(), key).Get);

		public Selector<TIn, TOut> Using(Func<TIn, object> key)
			=> new Memory<TIn, TOut>(Memory,
			                         new Source<TIn, TOut>(Memory, Subject.Get, _configure.Execute).Get,
			                         key).Then();
	}
}