using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	public sealed class ConfiguredMemoryStoreContext<TIn, TOut> : MemoryStoreContext<TIn, TOut>
	{
		readonly ICommand<ICacheEntry> _configure;

		public ConfiguredMemoryStoreContext(ISelect<TIn, ValueTask<TOut>> subject, IMemoryCache memory,
		                                    ICommand<ICacheEntry> configure)
			: base(subject, memory)
			=> _configure = configure;

		public OperationResultSelector<TIn, TOut> Using<T>(Func<TIn, string> key)
			=> Using(new Key<TIn>(A.Type<T>().AssemblyQualifiedName ?? throw new InvalidOperationException(), key).Get);

		public OperationResultSelector<TIn, TOut> Using(ISelect<TIn, object> key) => Using(key.Get);

		public OperationResultSelector<TIn, TOut> Using(Func<TIn, object> key)
			=> new Memory<TIn, TOut>(Memory,
			                         new Source<TIn, TOut>(Memory, Subject.Get, _configure.Execute).Get,
			                         key).Then();
	}
}