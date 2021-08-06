using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	class Any<T> : Materializer<T, bool>, IAny<T>
	{
		public Any(ISelect<IQueryable<T>, ValueTask<bool>> select) : base(select) {}

		public Any(Func<IQueryable<T>, ValueTask<bool>> select) : base(select) {}
	}

	public class Any<TIn, T> : Materialize<TIn, T, bool>
	{
		public Any(IQuery<TIn, T> query) : this(query, DefaultAny<T>.Default) {}

		protected Any(IQuery<TIn, T> query, IMaterializer<T, bool> materializer) : base(query, materializer) {}
	}
}