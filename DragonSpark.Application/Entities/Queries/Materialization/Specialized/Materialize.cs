using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialization.Specialized
{
	public class Materialize<T> : Resulting<Array<T>>
	{
		protected Materialize(IQueryable<T> source) : base(DefaultToArray<T>.Default.Then().Bind(source)) {}
	}
}
