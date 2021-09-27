using DragonSpark.Application.Entities.Editing;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Compiled
{
	public interface IForm<TIn, out T> : ISelect<In<TIn>, IAsyncEnumerable<T>> {}
}