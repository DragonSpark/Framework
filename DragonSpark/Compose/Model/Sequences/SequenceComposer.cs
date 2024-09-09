using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Compose.Model.Sequences;

public class SequenceComposer<_, T> : Composer<_, IEnumerable<T>>
{
	public SequenceComposer(ISelect<_, IEnumerable<T>> subject) : base(subject) {}

	public Composer<_, T> FirstAssigned() => FirstOrDefault(y => y is not null);

	public Composer<_, T> Only() => Select(Only<T>.Default);

	public Composer<_, T> FirstOrDefault(Func<T, bool> where) => Select(x => x.FirstOrDefault(where))!;

	public Composer<_, Array<T>> Result() => Select(x => x.Result());
}