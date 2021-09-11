using DragonSpark.Application;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Text;
using Microsoft.Extensions.Caching.Memory;
using Radzen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	class Class1 {}

	public sealed class RadzenCallback<T> : IAllocated<LoadDataArgs>
	{
		readonly IEvaluate<T> _evaluate;
		readonly bool         _includeCount;

		public RadzenCallback(IEvaluate<T> evaluate, bool includeCount = true)
		{
			_evaluate     = evaluate;
			_includeCount = includeCount;
		}

		public ulong Count { get; private set; }

		public IEnumerable<T> Current { get; private set; } = default!;

		public async Task Get(LoadDataArgs parameter)
		{
			var input = new QueryInput
			{
				Filter  = parameter.Filter,
				OrderBy = parameter.OrderBy,
				Partition = parameter.Skip.HasValue || parameter.Top.HasValue
					            ? new Partition(parameter.Skip, parameter.Top)
					            : null,
				IncludeTotalCount = _includeCount,
			};

			var evaluate = await _evaluate.Await(input);
			Current = evaluate;
			Count   = evaluate.Total ?? evaluate.Count.Grade();
		}
	}

	sealed class MemoryAwareActiveContents<T> : IActiveContents<T>
	{
		readonly IActiveContents<T>   _previous;
		readonly IMemoryCache         _memory;
		readonly IFormatter<Delegate> _key;

		public MemoryAwareActiveContents(IActiveContents<T> previous, IMemoryCache memory, RenderContentKey<T> key)
		{
			_previous = previous;
			_memory   = memory;
			_key      = key;
		}

		public IActiveContent<T> Get(Func<ValueTask<T>> parameter)
		{
			var previous = _previous.Get(parameter);
			var key      = _key.Get(parameter);
			var result   = new MemoryAwareActiveContent<T>(previous, _memory, key);
			return result;
		}
	}

	sealed class RenderContentKey<T> : IFormatter<Delegate>
	{
		readonly ICurrentPrincipal _current;
		readonly string            _type;

		public RenderContentKey(ICurrentPrincipal current)
			: this(current, A.Type<T>().AssemblyQualifiedName.Verify()) {}

		public RenderContentKey(ICurrentPrincipal current, string type)
		{
			_current = current;
			_type    = type;
		}

		public string Get(Delegate parameter)
			=> $"{parameter.Target.Verify().GetType().AssemblyQualifiedName}+{_type}+{_current.Get().UserName()}";
	}

	sealed class MemoryAwareActiveContent<T> : Resulting<T>, IActiveContent<T>
	{
		readonly IActiveContent<T> _previous;

		public MemoryAwareActiveContent(IActiveContent<T> previous, IMemoryCache memory, string key)
			: base(previous.Then()
			               .Accept()
			               .Then()
			               .Store()
			               .In(memory)
			               .For(TimeSpan.FromSeconds(3))
			               .Using<MemoryAwareActiveContent<T>>(key.Accept)
			               .Bind())
			=> _previous = previous;

		public bool HasValue => _previous.HasValue;
	}
}