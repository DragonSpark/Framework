namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public class First<TIn, T> : Materialize<TIn, T, T>
	{
		public First(IQuery<TIn, T> query) : this(query, FirstMaterializer<T>.Default) {}

		protected First(IQuery<TIn, T> query, IMaterializer<T, T> materializer) : base(query, materializer) {}
	}
}