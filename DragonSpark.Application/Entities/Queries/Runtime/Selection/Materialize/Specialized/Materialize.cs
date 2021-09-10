using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize.Specialized
{
	public class Materialize<T> : Resulting<Array<T>>
	{
		protected Materialize(IQueryable<T> source) : base(DefaultToArray<T>.Default.Then().Bind(source)) {}
	}
}
