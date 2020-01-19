using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences
{
	public interface IIterate<T> : ISelect<IEnumerable<T>, Store<T>> {}
}