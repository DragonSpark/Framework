using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Eventing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/mikoskinen/Blazor.EventAggregator
	/// </summary>
	public class EventAggregator : IEventAggregator, IDisposable
	{
		readonly List<Handler> _handlers = new();

		/// <inheritdoc />
		public virtual void Subscribe(object subscriber)
		{
			if (subscriber == null)
			{
				throw new ArgumentNullException(nameof(subscriber));
			}

			lock (_handlers)
			{
				if (!_handlers.Any(x => x.Matches(subscriber)))
				{
					_handlers.Add(new Handler(subscriber));
				}
			}
		}

		/// <inheritdoc />
		public virtual void Unsubscribe(object subscriber)
		{
			if (subscriber == null)
			{
				throw new ArgumentNullException(nameof(subscriber));
			}

			lock (_handlers)
			{
				foreach (var handler in _handlers.Where(x => x.Matches(subscriber)).ToArray())
				{
					_handlers.Remove(handler);
				}
			}
		}

		public virtual Task PublishAsync(object message)
		{
			if (message == null)
			{
				throw new ArgumentNullException(nameof(message));
			}

			Handler[] handlers;

			var type = message.GetType();

			lock (_handlers)
			{
				_handlers.RemoveAll(x => x.IsDead);
				handlers = _handlers.Where(x => x.Handles(type)).ToArray();
			}

			var tasks  = handlers.Select(h => h.Handle(type, message));
			var result = Task.WhenAll(tasks);
			return result;
		}


		sealed class Handler
		{
			readonly WeakReference                _reference;
			readonly Dictionary<Type, MethodInfo> _handlers = new();

			public Handler(object instance)
			{
				_reference = new WeakReference(instance);

				foreach (var @interface in instance.GetType()
				                                   .GetTypeInfo()
				                                   .ImplementedInterfaces.Where(x => x.IsGenericType &&
				                                                                     x.GetGenericTypeDefinition() ==
				                                                                     typeof(IHandle<>)))
				{
					var type   = @interface.GenericTypeArguments[0];
					var method = @interface.GetRuntimeMethod(nameof(IHandle<object>.HandleAsync), new[] { type });

					if (method != null)
					{
						_handlers[type] = method;
					}
				}
			}

			public bool IsDead => _reference.Target == null;

			public bool Matches(object instance) => _reference.Target == instance;

			public bool Handles(Type messageType) => _handlers.Any(x => x.Key.IsAssignableFrom(messageType));

			public async Task Handle(Type messageType, object message)
			{
				var target = _reference.Target;
				if (target != null)
				{
					var parameters = new[] { message };
					foreach (var (key, value) in _handlers)
					{
						if (key.IsAssignableFrom(messageType) && value.Invoke(target, parameters) is Task t)
						{
							await t;
						}
					}
				}
			}
		}

		public void Dispose()
		{
			_handlers.Clear();
		}
	}
}