using Microsoft.Practices.ServiceLocation;

namespace Common.EntityModel
{
	public static class EntityExtensions
	{
		public static TEntity WithExternalEntities<TEntity>( this TEntity target ) where TEntity : System.Windows.Ria.Entity
		{
			var result = WithExternalEntities( target, ServiceLocator.Current.GetInstance<IExternalEntityLoader>() );
			return result;
		}

		public static TEntity WithExternalEntities<TEntity>( this TEntity target, IExternalEntityLoader loader ) where TEntity : System.Windows.Ria.Entity
		{
			loader.LoadExternalEntities( target );
			return target;
		}
	}
}