using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Compose.Model.Sequences
{
	public class SequenceSelector<_, T> : Selector<_, IEnumerable<T>>
	{
		public SequenceSelector(ISelect<_, IEnumerable<T>> subject) : base(subject) {}

		public Selector<_, T> FirstAssigned() => FirstOrDefault(y => !(y is null));

		public Selector<_, T> Only() => Select(Only<T>.Default);

		public Selector<_, T> FirstOrDefault(Func<T, bool> where) => Select(x => x.FirstOrDefault(where))!;

		public Selector<_, Array<T>> Result() => Select(x => x.Result());
	}
}