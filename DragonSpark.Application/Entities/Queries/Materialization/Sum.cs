namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class Sum<T> : Materialize<T, decimal, decimal>
	{
		public Sum(Scoped.IQuery<T, decimal> query) : this(query, SumMaterializer.Default) {}

		protected Sum(Scoped.IQuery<T, decimal> query, IMaterializer<decimal, decimal> materializer)
			: base(query, materializer) {}
	}
}