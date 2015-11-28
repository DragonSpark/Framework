using AutoMapper;
using DragonSpark.Activation;
using System;

namespace DragonSpark.Extensions
{
	public class ObjectMappingContext<T>
	{
		public ObjectMappingContext( object source, T existing, Action<IMappingExpression> configuration )
		{
			Source = source;
			Existing = existing;
			Configuration = configuration;
		}

		public object Source { get; }

		public T Existing { get; }

		public Action<IMappingExpression> Configuration { get; }
	}

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
				
			/*Mapper.FindTypeMapFor( sourceType, destinationType ).Null( () =>
			{
				
			} );*/
			var expression = Mapper.CreateMap( sourceType, destinationType ).IgnoreUnassignable( sourceType, destinationType );
			expression.TypeMap.DestinationCtor = x => parameter.Existing ?? activator.Activate( x.DestinationType );
			parameter.Configuration.With( x => x( expression ) );

			var result = Mapper.DynamicMap( parameter.Source, sourceType, destinationType );
			return (T)result;
		}
	}
}