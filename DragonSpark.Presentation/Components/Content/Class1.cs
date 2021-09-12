using DragonSpark.Application;
using DragonSpark.Application.Components;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime;
using DragonSpark.Text;
using Microsoft.Extensions.Caching.Memory;
using Radzen;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Delegate = System.Delegate;

namespace DragonSpark.Presentation.Components.Content
{
	class Class1 {}

	public interface IRadzenData<out T> : IAllocated<LoadDataArgs>
	{
		public ulong Count { get; }

		public IEnumerable<T> Current { get; }
	}

	sealed class ValidatedRadzenData<T> : IRadzenData<T>
	{
		readonly IRadzenData<T>                  _previous;
		readonly IMutable<LoadDataArgs?>         _store;
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

		LoadDataArgsEqualityComparer() : this(StringComparer.InvariantCultureIgnoreCase) {}

		readonly IEqualityComparer<string> _comparer;

		public LoadDataArgsEqualityComparer(IEqualityComparer<string> comparer) => _comparer = comparer;

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
			code.Add(obj.OrderBy, _comparer);
			code.Add(obj.Filter, _comparer);
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

		public DataReferences(IMutable<LoadDataArgs?> variable,
		                      ConcurrentDictionary<IEvaluate<T>, IRadzenData<T>> store,
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

	sealed class PreRenderAwareActiveContents<T> : IActiveContents<T>
	{
		readonly IActiveContents<T>           _previous;
		readonly IsPreRendering               _condition;
		readonly MemoryAwareActiveContents<T> _memory;

		public PreRenderAwareActiveContents(IActiveContents<T> previous, IsPreRendering condition,
		                                    MemoryAwareActiveContents<T> memory)
		{
			_previous  = previous;
			_condition = condition;
			_memory    = memory;
		}

		public IActiveContent<T> Get(Func<ValueTask<T>> parameter)
		{
			var condition = _condition.Get();
			var content   = _previous.Get(parameter);
			var result = condition
				             ? new PreRenderActiveContent<T>(_condition, _memory.Get(parameter), content)
				             : content;
			return result;
		}
	}

	sealed class PreRenderActiveContent<T> : Validated<ValueTask<T>>, IActiveContent<T>
	{
		public PreRenderActiveContent(ICondition condition, IActiveContent<T> memory, IActiveContent<T> previous)
			: base(condition, memory, previous) {}
	}

	sealed class IsTracking : Variable<bool>
	{
		public IsTracking() : base(true) {}
	}

	sealed class IsPreRendering : AllCondition<None>, ICondition
	{
		public IsPreRendering(IsTracking tracking, ConnectionStartTime start)
			: base(A.Result(tracking).Then().Accept(),
			       Time.Default.WithinLast(PreRenderingWindow.Default).Then().Bind(start.Get).Accept()) {}
	}

	sealed class MemoryAwareActiveContents<T> : IActiveContents<T>
	{
		readonly IActiveContents<T> _previous;
		readonly IMemoryCache       _memory;
		readonly IRenderContentKey  _key;

		public MemoryAwareActiveContents(IMemoryCache memory, IRenderContentKey key)
			: this(ActiveContents<T>.Default, memory, key) {}

		[Candidate(false)]
		public MemoryAwareActiveContents(IActiveContents<T> previous, IMemoryCache memory, IRenderContentKey key)
		{
			_previous = previous;
			_memory   = memory;
			_key      = key;
		}

		public IActiveContent<T> Get(Func<ValueTask<T>> parameter)
		{
			var previous = _previous.Get(parameter);
			var key      = _key.Get(parameter);
			return new MemoryAwareActiveContent<T>(previous, _memory, key);
		}
	}

	sealed class PreRenderingWindow : DragonSpark.Model.Results.Instance<TimeSpan>
	{
		public static PreRenderingWindow Default { get; } = new();

		PreRenderingWindow() : base(TimeSpan.FromSeconds(10)) {}
	}

	sealed class StoreAwareRenderContentKey : IRenderContentKey
	{
		readonly IRenderContentKey _previous;
		readonly RenderContentKeys _store;

		public StoreAwareRenderContentKey(IRenderContentKey previous, RenderContentKeys store)
		{
			_previous = previous;
			_store    = store;
		}

		public string Get(Delegate parameter)
		{
			var previous = _previous.Get(parameter);
			_store.Add(previous);
			return previous;
		}
	}

	public interface IRenderContentKey : IFormatter<Delegate> {}

	sealed class RenderContentKeys : HashSet<string> {}

	sealed class RenderContentKey : IRenderContentKey
	{
		readonly ICurrentPrincipal _current;

		public RenderContentKey(ICurrentPrincipal current) => _current = current;

		public string Get(Delegate parameter)
			=> $"{parameter.Target.Verify().GetType().AssemblyQualifiedName}+{parameter.Method.ReturnType.AssemblyQualifiedName}+{_current.Get().UserName()}";
	}

	public interface IContentInteraction : ICommand {}

	sealed class ContentInteraction : IContentInteraction
	{
		readonly IsTracking        _switch;
		readonly IMemoryCache      _memory;
		readonly RenderContentKeys _keys;

		public ContentInteraction(IsTracking @switch, IMemoryCache memory, RenderContentKeys keys)
		{
			_switch = @switch;
			_memory = memory;
			_keys   = keys;
		}

		public void Execute(None parameter)
		{
			if (_switch.Get())
			{
				_switch.Execute(false);

				foreach (var key in _keys)
				{
					_memory.Remove(key);
				}
			}
			_keys.Clear();
		}
	}

	sealed class MemoryAwareActiveContent<T> : Resulting<T>, IActiveContent<T>
	{
		public MemoryAwareActiveContent(IActiveContent<T> previous, IMemoryCache memory, string key)
			: base(previous.Then()
			               .Accept()
			               .Then()
			               .Store()
			               .In(memory)
			               .For(PreRenderingWindow.Default.Get().Slide())
			               .Using(key.Accept)
			               .Bind()) {}
	}
}