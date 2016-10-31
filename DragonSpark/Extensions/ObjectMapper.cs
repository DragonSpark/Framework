using AutoMapper;
using AutoMapper.Configuration;
using DragonSpark.Activation;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Extensions
{
	public sealed class ObjectMapper<T> : ParameterizedSourceBase<ObjectMappingParameter<T>, T> where T : class
	{
		public static ObjectMapper<T> Default { get; } = new ObjectMapper<T>();
		ObjectMapper() : this( Activator.Default ) {}

		readonly IActivator activatorSource;

		readonly IArgumentCache<TypePair, IMapper> mappers = new StructuralCache<TypePair, IMapper>( Factory.Implementation.Get );

		public ObjectMapper( IActivator activatorSource )
		{
			this.activatorSource = activatorSource;
		}

		public override T Get( ObjectMappingParameter<T> parameter )
		{
			var destination = parameter.Destination ?? activatorSource.Get( parameter.Pair.DestinationType );
			var result = mappers.Get( parameter.Pair ).Map( parameter.Source, (T)destination );
			return result;
		}

		sealed class Factory : ParameterizedSourceBase<TypePair, IMapper>
		{
			public static IParameterizedSource<TypePair, IMapper> Implementation { get; } = new Factory();
			Factory() {}

			public override IMapper Get( TypePair parameter )
			{
				var expression = new MapperConfigurationExpression();
				expression.ForAllPropertyMaps( map => map.SourceMember == null || !map.DestinationPropertyType.Adapt().IsAssignableFrom( map.SourceMember.GetMemberType() ), ( map, _ ) => map.Ignored = true );
				expression.CreateMap( parameter.SourceType, parameter.DestinationType, MemberList.Destination );
				expression.ForAllMaps( ( map, mappingExpression ) => mappingExpression.ForAllMembers( option => option.Condition( ( source, destination, sourceValue ) => sourceValue.IsAssigned() ) ) );
				var result = new MapperConfiguration( expression ).CreateMapper();
				return result;
			}
		}
	}
}