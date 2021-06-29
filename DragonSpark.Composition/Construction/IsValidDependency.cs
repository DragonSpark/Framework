using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using LightInject;
using NetFabric.Hyperlinq;
using System;
using System.Linq;

namespace DragonSpark.Composition.Construction
{
	sealed class IsValidDependency : ICondition<Type>
	{
		readonly IServiceRegistry _registry;
		readonly Array<Type>      _types;

		public IsValidDependency(IServiceRegistry registry, Array<Type> types)
		{
			_registry = registry;
			_types    = types;
		}

		public bool Get(Type parameter)
			=> !parameter.IsConstructedGenericType || !_types.Open()
			                                                 .Contains(parameter.GetGenericTypeDefinition())
			                                       || _registry.AvailableServices.Introduce(parameter)
			                                                   .Any(x => x.Item1.ServiceType == x.Item2);
	}
}