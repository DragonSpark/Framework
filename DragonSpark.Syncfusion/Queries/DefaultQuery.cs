using DragonSpark.Application.Entities.Queries.Materialize;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class DefaultQuery<T> : Instances<IQuery<T>>
	{
		public DefaultQuery(ICount<T> counting)
			: base(Search<T>.Default, Sort<T>.Default, Where<T>.Default, new Count<T>(counting), Skip<T>.Default,
			       Take<T>.Default) {}
	}
}