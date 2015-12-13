using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Linq;

namespace DragonSpark.Activation.FactoryModel
{
	public class FactoryReflectionSupport
	{
		public static FactoryReflectionSupport Instance { get; } = new FactoryReflectionSupport();
		static readonly TypeAdapter[] 
			Types = new[] { typeof(IFactory<>), typeof(IFactory<,>) }.Select( type => type.Adapt() ).ToArray(),
			BasicTypes = new[] { typeof(IFactory), typeof(IFactoryWithParameter) }.Select( type => type.Adapt() ).ToArray();

		public Type GetResultType( Type factoryType )
		{
			var result = Get( factoryType, types => types.Last(), Types );
			return result;
		}

		public Type GetFactoryType( Type itemType )
		{
			var name = $"{itemType.Name}Factory";
			var result = itemType.Assembly().DefinedTypes.AsTypes().ToArray().With( types => types.Where( info => info.Name == name ).Only() 
				??
				types
					.Where( x => BasicTypes.Any( extension => extension.IsAssignableFrom( x ) ) )
					.Where( type => GetResultType( type ) == itemType ).Only() );
			return result;
		}

		static Type Get( Type factoryType, Func<Type[],Type> selector, params TypeAdapter[] typesToCheck )
		{
			var result = factoryType
				.Adapt()
				.GetAllInterfaces()
				.AsTypeInfos()
				.Where( type => type.IsGenericType && typesToCheck.Any( extension => extension.IsAssignableFrom( type.GetGenericTypeDefinition() ) ) )
				.Select( type => selector( type.GenericTypeArguments ) )
				.FirstOrDefault();
			return result;
		}

		/*public TypeAdapter GetParameterType( TypeAdapter factoryType )
		{
			var result = Get( factoryType, types => types.First(), Types.Last() );
			return result;
		}*/
	}
}