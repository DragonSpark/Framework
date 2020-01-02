using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Environment;
using System;

namespace DragonSpark.Aspects
{
	sealed class AspectRegistrations<TIn, TOut> : IArray<Array<Type>, IAspect<TIn, TOut>>
	{
		readonly ISelect<object, IAspect<TIn, TOut>> _adapter;
		readonly Func<Array<IRegistration>>          _registrations;
		readonly IStorage<IAspect<TIn, TOut>>        _stores;

		public AspectRegistrations(IRegistry<IRegistration> registry)
			: this(Leases<IAspect<TIn, TOut>>.Default, registry.Get, AdapterAspects<TIn, TOut>.Default) {}

		public AspectRegistrations(IStorage<IAspect<TIn, TOut>> stores, Func<Array<IRegistration>> registrations,
		                           ISelect<object, IAspect<TIn, TOut>> adapter)
		{
			_stores        = stores;
			_registrations = registrations;
			_adapter       = adapter;
		}

		public Array<IAspect<TIn, TOut>> Get(Array<Type> parameter)
		{
			var       registrations = _registrations();
			var       to            = registrations.Length;
			using var elements      = _stores.Session(to);
			var       source        = elements.Store;
			var       count         = 0u;

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