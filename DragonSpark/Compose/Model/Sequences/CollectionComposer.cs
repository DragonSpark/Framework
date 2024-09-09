using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Collections;
using System.Collections.Generic;

namespace DragonSpark.Compose.Model.Sequences;

public class CollectionComposer<_, T> : SequenceComposer<_, T>
{
	readonly ISelect<_, ICollection<T>> _subject;

	public CollectionComposer(ISelect<_, ICollection<T>> subject) : base(subject) => _subject = subject;

	public Composer<_, ICollection<T>> Subject => new(_subject);

	public ConditionComposer<_> HasAny() => new(_subject.Select(HasAny<T>.Default));

	public ConditionComposer<_> HasNone() => new(_subject.Select(HasNone<T>.Default));
}