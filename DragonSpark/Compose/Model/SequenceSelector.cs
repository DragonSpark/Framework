using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Compose.Model
{
	public class SequenceSelector<_, T> : Selector<_, IEnumerable<T>>
	{
		public SequenceSelector(ISelect<_, IEnumerable<T>> subject) : base(subject) {}

		public Selector<_, T> FirstAssigned() => Select(x => x.FirstOrDefault(y => !(y is null)));

		public Selector<_, T> Only() => Select(DragonSpark.Model.Sequences.Query.Only<T>.Default);

		public Selector<_, T> FirstOrDefault() => Select(x => x.FirstOrDefault());

		public Selector<_, Array<T>> Result() => Select(x => x.Result());
	}
}