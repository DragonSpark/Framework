using DragonSpark.Application;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.Extensions.Caching.Memory;
using Radzen;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	class Class1 {}

	public interface IRadzenData<T> : IAllocated<LoadDataArgs>
	{
		public ulong Count { get; }

		public IEnumerable<T> Current { get; }
	}

	sealed class ValidatedRadzenData<T> : IRadzenData<T>
	{
		readonly IRadzenData<T>                  _previous;
		readonly IMutable<LoadDataArgs?>          _store;
		readonly IEqualityComparer<LoadDataArgs> _comparer;

		public ValidatedRadzenData(IRadzenData<T> previous, IMutable<LoadDataArgs?> store)
			: this(previous, store, LoadDataArgsEqualityComparer.Default) {}

		public ValidatedRadzenData(IRadzenData<T> previous, IMutable<LoadDataArgs?> store,
		                           IEqualityComparer<LoadDataArgs> comparer)
		{
			_previous = previous;
			_store    = store;
			_comparer = comparer;
		}

		public Task Get(LoadDataArgs parameter)
		{
			var current = _store.Get();
			if (!_comparer.Equals(current, parameter))
			{
				_store.Execute(parameter);
				return _previous.Get(parameter);
			}
			return Task.CompletedTask;
		}

		public ulong Count => _previous.Count;

		public IEnumerable<T> Current => _previous.Current;
	}

	sealed class LoadDataArgsEqualityComparer : IEqualityComparer<LoadDataArgs>
	{
		public static LoadDataArgsEqualityComparer Default { get; } = new();

		LoadDataArgsEqualityComparer() {}

		public bool Equals(LoadDataArgs? x, LoadDataArgs? y)
			=> ReferenceEquals(x, y) || !ReferenceEquals(x, null)
			   &&
			   !ReferenceEquals(y, null)
			   &&
			   x.GetType() == y.GetType()
			   &&
			   x.Skip == y.Skip && x.Top == y.Top
			   && string.Equals(x.OrderBy, y.OrderBy, StringComparison.InvariantCultureIgnoreCase)
			   &&
			   string.Equals(x.Filter, y.Filter, StringComparison.InvariantCultureIgnoreCase);

		public int GetHashCode(LoadDataArgs obj)
		{
			var code = new HashCode();
			code.Add(obj.Skip);
			code.Add(obj.Top);
			code.Add(obj.OrderBy, StringComparer.InvariantCultureIgnoreCase);
			code.Add(obj.Filter, StringComparer.InvariantCultureIgnoreCase);
			var result = code.ToHashCode();
			return result;
		}
	}

	public sealed class RadzenData<T> : IRadzenData<T>
	{
		readonly IEvaluate<T> _evaluate;
		readonly bool         _includeCount;

		public RadzenData(IEvaluate<T> evaluate, bool includeCount = true)
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


	sealed class DataReferences<T> : DecoratedTable<IEvaluate<T>, IRadzenData<T>>, ICommand
	{
		readonly IMutable<LoadDataArgs?>                   _variable;
		readonly IDictionary<IEvaluate<T>, IRadzenData<T>> _store;

		public DataReferences(bool count)
			: this(count, new Variable<LoadDataArgs?>()) {}

		public DataReferences(bool count, IMutable<LoadDataArgs?> store)
			: this(store, new ConcurrentDictionary<IEvaluate<T>, IRadzenData<T>>(),
			       x => new ValidatedRadzenData<T>(new RadzenData<T>(x, count), store)) {}

		public DataReferences(IMutable<LoadDataArgs?> variable, ConcurrentDictionary<IEvaluate<T>, IRadzenData<T>> store,
		                      Func<IEvaluate<T>, IRadzenData<T>> factory)
			: this(variable, store, factory.ToConcurrentTable(store)) {}

		public DataReferences(IMutable<LoadDataArgs?> variable, IDictionary<IEvaluate<T>, IRadzenData<T>> store,
		                      ITable<IEvaluate<T>, IRadzenData<T>> table)
			: base(table)
		{
			_variable = variable;
			_store    = store;
		}

		public void Execute(None parameter)
		{
			_variable.Execute(null);
			_store.Clear();
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