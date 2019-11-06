using System;
using DragonSpark.Model.Sequences.Query;

namespace DragonSpark.Model.Selection.Adapters
{
	public class OpenArraySelector<_, T> : CollectionSelector<_, T>
	{
		readonly ISelect<_, T[]> _subject;

		public OpenArraySelector(ISelect<_, T[]> subject) : base(subject) => _subject = subject;

		public ConditionSelector<_> AllAre(Func<T, bool> condition)
			=> new ConditionSelector<_>(_subject.Select(new AllItemsAre<T>(condition)));

		public OpenArraySelector<_, T> Sort()
			=> new OpenArraySelector<_, T>(_subject.Select(SortAlteration<T>.Default));
	}
}