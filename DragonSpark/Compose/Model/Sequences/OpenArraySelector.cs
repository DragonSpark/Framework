﻿using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Query;
using System;

namespace DragonSpark.Compose.Model.Sequences;

public class OpenArraySelector<_, T> : CollectionSelector<_, T>
{
	readonly ISelect<_, T[]> _subject;

	public OpenArraySelector(ISelect<_, T[]> subject) : base(subject) => _subject = subject;

	public new Selector<_, T[]> Subject => new(_subject);

	public ConditionSelector<_> AllAre(Func<T, bool> condition) => new(_subject.Select(new AllItemsAre<T>(condition)));

	public OpenArraySelector<_, T> Sort() => new(_subject.Select(SortAlteration<T>.Default));
}