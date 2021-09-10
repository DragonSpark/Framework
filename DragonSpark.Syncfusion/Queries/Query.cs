using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Model.Operations;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Query<T> : Altering<Parameter<T>>, IQuery<T>
	{
		public Query(IMaterialization<T> materialization) : base(new DefaultQuery<T>(materialization.Counting)) {}
	}
}