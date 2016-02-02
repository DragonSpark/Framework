using AutoMapper;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Extensions
{
	public class ObjectMappingFactory<T> : FactoryBase<ObjectMappingParameter<T>, T> where T : class
	{
		readonly Activator.Get activator;

		public ObjectMappingFactory() : this( Activator.GetCurrent ) {}

		ObjectMappingFactory( Activator.Get activator )
		{
			this.activator = activator;
		}

		protected override T CreateItem( ObjectMappingParameter<T> parameter )
		{
			var sourceType = parameter.Source.GetType();
			var destinationType = parameter.Existing?.GetType() ?? ( typeof(T) == typeof(object) ? sourceType : typeof(T) );
				
			var configuration = new MapperConfiguration( mapper =>
			{
				var map = mapper.CreateMap( sourceType, destinationType ).IgnoreUnassignable();
				map.TypeMap.DestinationCtor = x => parameter.Existing ?? activator().Activate( x.DestinationType );
				parameter.Configuration.With( x => x( map ) );
			} );

			configuration.To<IProfileExpression>().CreateMissingTypeMaps = true;

			var result = configuration.CreateMapper().Map( parameter.Source, sourceType, destinationType );
			return (T)result;
		}
	}
}