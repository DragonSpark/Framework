using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using System;

namespace DragonSpark.Aspects
{
	sealed class AspectLocator<TIn, TOut> : ISelect<Type, IAspect<TIn, TOut>>
	{
		readonly ISelect<Type, Array<Type>>              _implementations;
		readonly IArray<Array<Type>, IAspect<TIn, TOut>> _registrations;

		public AspectLocator(IArray<Array<Type>, IAspect<TIn, TOut>> registrations)
			: this(SelectionImplementations.Default, registrations) {}

		public AspectLocator(ISelect<Type, Array<Type>> implementations,
		                         IArray<Array<Type>, IAspect<TIn, TOut>> registrations)
		{
			_implementations = implementations;
			_registrations   = registrations;
		}

		public IAspect<TIn, TOut> Get(Type parameter)
		{
			var implementations = _implementations.Get(parameter);
			var length          = implementations.Length;
			if (implementations.Length > 0)
			{
				var store = new DynamicStore<IAspect<TIn, TOut>>(32);
				for (var i = 0u; i < length; i++)
				{
					var registrations = _registrations.Get(implementations[i].GenericTypeArguments);
					store = store.Add(new Store<IAspect<TIn, TOut>>(registrations));
				}

				return new CompositeAspect<TIn, TOut>(store.Get().Instance);
			}

			return EmptyAspect<TIn, TOut>.Default;
		}
	}
}