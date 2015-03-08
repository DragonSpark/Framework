using System;
using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class AssociationQueryShaper : IQueryShaper
	{
		readonly object owner;
		readonly PropertyInfo info;

		public AssociationQueryShaper( object owner, PropertyInfo info )
		{
			this.owner = owner;
			this.info = info;
		}

		EntityQuery<TEntity> IQueryShaper.Shape<TEntity>( EntityQuery<TEntity> query )
		{
			var properties = info.DetermineAssociationProperties();
			var factory = Activator.CreateInstance( typeof(FilterExpressionFactory<,>).MakeGenericType( owner.GetType(), info.ResolveEntityType() ), properties ).To<IFactory>();
			var o = factory.Create( typeof(object), owner );
			var predicate = (System.Linq.Expressions.Expression<Func<TEntity, bool>>)o;
			var result = query.Where( predicate );
			return result;
		}
	}
}