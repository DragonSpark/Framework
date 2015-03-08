using System;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
    public class LoadEntityOperationCommand<TEntity> : EntityOperationCommmandBase where TEntity : System.ServiceModel.DomainServices.Client.Entity
    {
        readonly DomainContext context;
        readonly EntityQuery<TEntity> query;
        readonly LoadBehavior loadBehavior;
        readonly Action<LoadOperation<TEntity>> callback;

        public LoadEntityOperationCommand( DomainContext context, EntityQuery<TEntity> query, LoadBehavior loadBehavior = LoadBehavior.MergeIntoCurrent, Action<LoadOperation<TEntity>> callback = null )
        {
            this.context = context;
            this.query = query;
            this.loadBehavior = loadBehavior;
            this.callback = callback;
        }

        protected override OperationBase ResolveOperation()
        {
            var result = context.Load( query, loadBehavior, callback, this );
            return result;
        }
    }
}