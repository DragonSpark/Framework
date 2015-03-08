using System;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
	public class LoadSingleEntityOperationCommand<TEntity> : LoadEntityOperationCommand<TEntity> where TEntity : System.ServiceModel.DomainServices.Client.Entity
	{
		public LoadSingleEntityOperationCommand( DomainContext context, EntityQuery<TEntity> query, Action<TEntity> assignment, LoadBehavior loadBehavior = LoadBehavior.MergeIntoCurrent ) : base( context, query, loadBehavior, x => x.HasError.IsFalse( () => assignment( x.Entities.SingleOrDefault() ) ) )
		{}
	}
}