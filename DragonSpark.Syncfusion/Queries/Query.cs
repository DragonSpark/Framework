using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Query<T> : IQuery<T>
	{
		public static Query<T> Default { get; } = new Query<T>();

		Query() : this(DefaultQuery<T>.Default) {}

		readonly Array<IQuery<T>> _alterations;

		public Query(Array<IQuery<T>> alterations) => _alterations = alterations;

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter) => _alterations.Open().Alter(parameter);
	}
}