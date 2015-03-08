using System;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Entity
{
	public static class DomainContextExtensions
	{
		// static readonly MethodInfo CreateMethod = typeof(DomainContextExtensions).GetMethod( "Create", DragonSparkBindingOptions.AllProperties );

		public static EntitySet DetermineEntitySet<TEntity>( this DomainContext target )
		{
			var result = target.DetermineEntitySet( typeof(TEntity) );
			return result;
		}

		public static EntitySet DetermineEntitySet( this DomainContext target, Type type )
		{
			try
			{
				return type.Transform( target.EntityContainer.GetEntitySet );
			}
			catch ( InvalidOperationException )
			{
				return type.Transform( x => x.BaseType.Transform( y => DetermineEntitySet( target, y ) ) );
			}
		}

		public static TContext Clearing<TContext>( this TContext target, params Type[] types ) where TContext : DomainContext
		{
			types.Apply( x => target.DetermineEntitySet( x ).Clear() );
			return target;
		}


		/*public static IEntitySetView CreateView( this DomainContext target, Type entityType, int pageSize = 10 )
		{
			var result = CreateMethod.MakeGenericMethod( entityType ).Invoke( null, new object[] { target, pageSize } ).To<IEntitySetView>();
			return result;
		}

		static IEntitySetView Create<TEntity>( DomainContext target, int pageSize = 10 ) where TEntity : System.ServiceModel.DomainServices.Client.Entity
		{
			var result = new EntitySetView<TEntity>( target, pageSize );
			return result;
		}*/
	}
}