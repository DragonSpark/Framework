using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, Store<T>> {}
}