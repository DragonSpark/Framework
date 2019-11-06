using System.Collections.Generic;
using DragonSpark.Model.Sequences.Collections;

namespace DragonSpark.Model.Selection.Adapters
{
	public class CollectionSelector<_, T> : SequenceSelector<_, T>
	{
		readonly ISelect<_, ICollection<T>> _subject;

		public CollectionSelector(ISelect<_, ICollection<T>> subject) : base(subject) => _subject = subject;

		public ConditionSelector<_> HasAny() => new ConditionSelector<_>(_subject.Select(HasAny<T>.Default));

		public ConditionSelector<_> HasNone() => new ConditionSelector<_>(_subject.Select(HasNone<T>.Default));
	}
}