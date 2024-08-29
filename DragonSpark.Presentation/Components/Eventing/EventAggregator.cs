using DragonSpark.Runtime;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Eventing;

/// <summary>
/// ATTRIBUTION: https://github.com/mikoskinen/Blazor.EventAggregator
/// </summary>
sealed class EventAggregator : IEventAggregator, IDisposable
{
	readonly List<WeakReference> _handlers;

	public EventAggregator() : this(new()) {}

	public EventAggregator(List<WeakReference> handlers) => _handlers = handlers;

	/// <inheritdoc />
	public void Subscribe(object subscriber)
	{
		if (_handlers.All(x => x.Target != subscriber))
		{
			_handlers.Add(new(subscriber));
		}
	}

	/// <inheritdoc />
	public void Unsubscribe(object subscriber)
	{
		using var lease = _handlers.AsValueEnumerable()
		                           .Where(x => x.Target == subscriber)
		                           .ToArray(ArrayPool<WeakReference>.Shared, true);
		foreach (var handler in lease)
		{
			_handlers.Remove(handler);
		}
	}

	public Task Publish<T>(T message) where T : class
	{
		using var all = _handlers.AsValueEnumerable().ToArray(ArrayPool<WeakReference>.Shared, true);

		using var builder = ArrayBuilder.New<Task>(all.Length);
		foreach (var handler in all)
		{
			if (handler.IsAlive)
			{
				if (handler.Target is IHandle<T> handle)
				{
					builder.UncheckedAdd(handle.HandleAsync(message));
				}
			}
			else
			{
				_handlers.Remove(handler);
			}
		}

		return Task.WhenAll(builder.AsSpan().ToArray());
	}

	public void Dispose()
	{
		_handlers.Clear();
	}
}