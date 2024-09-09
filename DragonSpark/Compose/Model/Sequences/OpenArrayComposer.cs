using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Query;
using System;

namespace DragonSpark.Compose.Model.Sequences;

public class OpenArrayComposer<_, T> : CollectionComposer<_, T>
{
	readonly ISelect<_, T[]> _subject;

	public OpenArrayComposer(ISelect<_, T[]> subject) : base(subject) => _subject = subject;

	public new Composer<_, T[]> Subject => new(_subject);

	public ConditionComposer<_> AllAre(Func<T, bool> condition) => new(_subject.Select(new AllItemsAre<T>(condition)));

	public OpenArrayComposer<_, T> Sort() => new(_subject.Select(SortAlteration<T>.Default));
}