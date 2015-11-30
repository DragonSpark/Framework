using AutoMapper;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Extensions
{
	public class ObjectMappingFactory<T> : FactoryBase<ObjectMappingContext<T>, T> where T : class
	{
		readonly IActivator activator;

		public ObjectMappingFactory() : this( SystemActivator.Instance )
		{}

		public ObjectMappingFactory( IActivator activator )
		{
			this.activator = activator;
		}

		protected override T CreateItem( ObjectMappingContext<T> parameter )
		{
			var sourceType = parameter.Source.GetType();
			var destinationType = parameter.Existing?.GetType() ?? ( typeof(T) == typeof(object) ? sourceType : typeof(T) );
				
			lock ( destinationType )
			{
				var expression = Mapper.CreateMap( sourceType, destinationType ).IgnoreUnassignable( sourceType, destinationType );
				expression.TypeMap.DestinationCtor = x => parameter.Existing ?? activator.Activate( x.DestinationType );
				parameter.Configuration.With( x => x( expression ) );

				var result = Mapper.DynamicMap( parameter.Source, sourceType, destinationType );
				return (T)result;
			}
		}
	}
}