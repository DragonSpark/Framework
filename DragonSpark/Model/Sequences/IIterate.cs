using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	public interface IIterate<T> : ISelect<IEnumerable<T>, Store<T>> {}
}