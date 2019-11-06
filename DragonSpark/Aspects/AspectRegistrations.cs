using System;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Aspects
{
	sealed class AspectRegistrations<TIn, TOut> : IArray<Array<Type>, IAspect<TIn, TOut>>
	{
		public static AspectRegistrations<TIn, TOut> Default { get; } = new AspectRegistrations<TIn, TOut>();

		AspectRegistrations() : this(Leases<IAspect<TIn, TOut>>.Default, Implementations.Registrations,
		                             AdapterAspects<TIn, TOut>.Default) {}

		readonly ISelect<object, IAspect<TIn, TOut>> _adapter;
		readonly Func<Array<IRegistration>>          _registrations;

		readonly IStores<IAspect<TIn, TOut>> _stores;

		public AspectRegistrations(IStores<IAspect<TIn, TOut>> stores, Func<Array<IRegistration>> registrations,
		                           ISelect<object, IAspect<TIn, TOut>> adapter)
		{
			_stores        = stores;
			_registrations = registrations;
			_adapter       = adapter;
		}

		public Array<IAspect<TIn, TOut>> Get(Array<Type> parameter)
		{
			var registrations = _registrations();
			var to            = registrations.Length;
			var elements      = _stores.Get(to);
			var source        = elements.Instance;
			var count         = 0u;

			for (var i = 0u; i < to; i++)
			{
				var registration = registrations[i];
				if (registration.Condition.Get(parameter))
				{
					var instance = registration.Get(parameter);
					source[count++] = instance is IAspect<TIn, TOut> aspect ? aspect : _adapter.Get(instance);
				}
			}

			return source.CopyInto(new IAspect<TIn, TOut>[count], 0, count);
		}
	}
}