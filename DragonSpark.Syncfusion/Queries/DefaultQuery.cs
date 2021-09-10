using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Model.Operations;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class DefaultQuery<T> : Alterings<Parameter<T>>, IQuery<T>
	{
		public DefaultQuery(ICount<T> counting)
			: base(
				  // Body:
				  Search<T>.Default, Where<T>.Default, Sort<T>.Default,

				  // Count:
				  new Count<T>(counting),

				  // Partition:
				  Skip<T>.Default, Take<T>.Default) {}
	}
}