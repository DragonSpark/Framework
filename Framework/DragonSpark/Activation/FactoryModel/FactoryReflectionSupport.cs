using System;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;

namespace DragonSpark.Activation.FactoryModel
{
	class FactoryReflectionSupport
	{
		public static FactoryReflectionSupport Instance { get; } = new FactoryReflectionSupport();
		static TypeExtension[] Types { get; } = new[] { typeof(IFactory<>), typeof(IFactory<,>) }.Select( type => TypeExtensions.Extend( (Type)type ) ).ToArray();

		public TypeExtension GetResultType( TypeExtension factoryType )
		{
			var result = Get( factoryType, types => types.Last(), Types );
			return result;
		}

		Type Get( TypeExtension factoryType, Func<Type[],Type> selector, params TypeExtension[] typesToCheck )
		{
			var result = factoryType.GetAllInterfaces().AsTypeInfos().Where( type => type.IsGenericType && typesToCheck.Any( extension => extension.IsAssignableFrom( type.GetGenericTypeDefinition() ) ) ).Select( type => selector( type.GenericTypeArguments ) ).FirstOrDefault();
			return result;
		}

		public TypeExtension GetParameterType( TypeExtension factoryType )
		{
			var result = Get( factoryType, types => types.First(), Types.Last() );
			return result;
		}
	}
}