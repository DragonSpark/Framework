using System.ServiceModel.DomainServices.Client;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;
using Microsoft.Windows.Data.DomainServices;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
    public class LoadEntitySetSourceOperation : LoadSourceOperation<EntitySetCollectionViewSource>
    {
        protected override EntityQuery<TEntity> DetermineQuery<TEntity>()
        {
            var methodName = ContextChecked.EntitySetService.GetQueryName( ContextChecked.EntitySets.SelectedItem, ContextChecked.ProfileViewName );
            var query = QueryGenerator.Instance.Generate( ContextChecked.EntityType, ContextChecked.DomainContext, methodName, ContextChecked.Query.ToEnumerable<object>() ).To<EntityQuery<TEntity>>();
            var result = ContextChecked.View.Transform( x => query.SortAndPageBy( x ), () => query );
            return result;
        }
    }
}