using DragonSpark.Application.Entities.Queries.Materialize;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Query<T> : IQuery<T>
	{
		readonly Array<IQuery<T>> _alterations;

		public Query(IMaterialization<T> materialization) : this(new DefaultQuery<T>(materialization.Counting)) {}

		public Query(Array<IQuery<T>> alterations) => _alterations = alterations;

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter) => _alterations.Open().Alter(parameter);
	}
}