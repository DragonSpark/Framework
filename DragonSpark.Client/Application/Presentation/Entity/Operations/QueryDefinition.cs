using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Markup;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
    [ContentProperty( "Parameters" )]
    public class QueryDefinition : ViewAwareObject
    {
        public string QueryName
        {
            get { return queryName; }
            set { SetProperty( ref queryName, value, () => QueryName ); }
        }	string queryName;

        public ViewAwareCollection<object> Parameters
        {
            get { return parameters; }
        }	readonly ViewAwareCollection<object> parameters = new ViewAwareCollection<object>();

        public ViewAwareCollection<IQueryShaper> Shapers
        {
            get { return shapers; }
        }	readonly ViewAwareCollection<IQueryShaper> shapers = new ViewAwareCollection<IQueryShaper>();
		
        public EntityQuery<TEntity> CreateQuery<TEntity>( DomainContext context ) where TEntity : System.ServiceModel.DomainServices.Client.Entity
        {
            var result = QueryGenerator.Instance.Generate( typeof(TEntity), context, QueryName, Parameters, Shapers.NotNull().ToArray() ).To<EntityQuery<TEntity>>();
            return result;
        }
    }
}