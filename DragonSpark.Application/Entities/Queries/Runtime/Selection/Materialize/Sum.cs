using DragonSpark.Application.Entities.Queries.Runtime.Materialize;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize
{
	public class Sum<T> : Materialize<T, decimal, decimal>
	{
		public Sum(IQuery<T, decimal> query) : this(query, SumMaterializer.Default) {}

		protected Sum(IQuery<T, decimal> query, IMaterializer<decimal, decimal> materializer)
			: base(query, materializer) {}
	}
}