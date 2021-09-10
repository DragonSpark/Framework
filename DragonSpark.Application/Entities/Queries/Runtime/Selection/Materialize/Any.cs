using DragonSpark.Application.Entities.Queries.Runtime.Materialize;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize
{
	public class Any<TIn, T> : Materialize<TIn, T, bool>
	{
		public Any(IQuery<TIn, T> query) : this(query, DefaultAny<T>.Default) {}

		protected Any(IQuery<TIn, T> query, IMaterializer<T, bool> materializer) : base(query, materializer) {}
	}}
