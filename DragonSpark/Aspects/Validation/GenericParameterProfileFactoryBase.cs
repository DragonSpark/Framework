using System;
using DragonSpark.TypeSystem.Generics;

namespace DragonSpark.Aspects.Validation
{
	public abstract class GenericParameterProfileFactoryBase : GenericInvocationFactory<object, IParameterValidationAdapter>
	{
		protected GenericParameterProfileFactoryBase( Type genericTypeDefinition, Type owningType, string methodName ) : base( genericTypeDefinition, owningType, methodName ) {}
	}
}