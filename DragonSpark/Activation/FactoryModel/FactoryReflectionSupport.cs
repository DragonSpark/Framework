using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Aspects;

namespace DragonSpark.Activation.FactoryModel
{
	public class FactoryReflectionSupport
	{
		public static FactoryReflectionSupport Instance { get; } = new FactoryReflectionSupport();
		readonly static TypeAdapter[] 
			Types = new[] { typeof(IFactory<>), typeof(IFactory<,>) }.Select( type => type.Adapt() ).ToArray(),
			BasicTypes = new[] { typeof(IFactory), typeof(IFactoryWithParameter) }.Select( type => type.Adapt() ).ToArray();

		[Cache]
		public static Type GetResultType( Type factoryType ) => Get( factoryType, types => types.Last(), Types );

		[Cache]
		public Type GetFactoryType( Type itemType, Type referenceType = null )
		{
			var name = $"{itemType.Name}Factory";
			var reference = referenceType ?? itemType;
			var result = new[] { reference, itemType }.Distinct().FirstWhere( candidate => LocateFactoryType( name, itemType, candidate ) );
			return result;
		}

		static Type LocateFactoryType( string name, Type itemType, Type candidate )
		{
			var result =
				candidate.GetTypeInfo().DeclaredNestedTypes.AsTypes().Where( info => info.Name == name ).Only()
				??
				candidate.Assembly().DefinedTypes.AsTypes().ToArray().With( types =>
					types.Where( info => info.Name == name ).Only()
					??
					types
						.Where( x => BasicTypes.Any( extension => extension.IsAssignableFrom( x ) ) )
						.Where( type => GetResultType( type ) == itemType ).Only()
					);
			return result;
		}

		static Type Get( Type factoryType, Func<Type[],Type> selector, params TypeAdapter[] typesToCheck )
		{
			var result = factoryType.Append( factoryType.Adapt().GetAllInterfaces() )
				.AsTypeInfos()
				.Where( type => type.IsGenericType && typesToCheck.Any( extension => extension.IsAssignableFrom( type.GetGenericTypeDefinition() ) ) )
				.Select( type => selector( type.GenericTypeArguments ) )
				.FirstOrDefault();
			return result;
		}

		[Cache]
		public Type GetParameterType( Type factoryType ) => Get( factoryType, types => types.First(), Types.Last().Adapt() );
	}
}