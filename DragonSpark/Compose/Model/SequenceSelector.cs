using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Compose.Model
{
	public class SequenceSelector<_, T> : Selector<_, IEnumerable<T>>
	{
		public SequenceSelector(ISelect<_, IEnumerable<T>> subject) : base(subject) {}
	}
}