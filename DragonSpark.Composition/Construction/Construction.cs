using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using LightInject;
using System;
using Activator = DragonSpark.Runtime.Activation.Activator;

namespace DragonSpark.Composition.Construction
{
	sealed class Construction : IConstructionInfoProvider
	{
		readonly IConstructionInfoProvider _provider;
		readonly ICondition<Type>          _condition;
		readonly IActivator                _activator;

		public Construction(IConstructionInfoProvider provider)
			: this(provider, CanActivate.Default, Activator.Default) {}

		public Construction(IConstructionInfoProvider provider, ICondition<Type> condition,
		                    IActivator activator)
		{
			_provider  = provider;
			_condition = condition;
			_activator = activator;
		}

		public ConstructionInfo? GetConstructionInfo(Registration registration)
		{
			try
			{
				return _provider.GetConstructionInfo(registration);
			}
			catch (InvalidOperationException)
			{
				if (_condition.Get(registration.ImplementingType))
				{
					var instance = _activator.Get(registration.ImplementingType);
					registration.FactoryExpression = new Func<IServiceFactory, object>(instance.Accept);
					if (registration is ServiceRegistration service)
					{
						service.Value = instance;
					}

					return new ConstructionInfo { FactoryDelegate = registration.FactoryExpression };
				}

				throw;
			}
		}
	}
}