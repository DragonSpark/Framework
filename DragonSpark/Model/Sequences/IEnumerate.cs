using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, Store<T>> {}
}