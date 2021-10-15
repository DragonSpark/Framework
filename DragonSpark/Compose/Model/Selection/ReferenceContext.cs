using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Compose.Model.Selection;

public class ReferenceContext<_, T> where _ : class
{
	readonly ISelect<_, T>                     _subject;
	readonly ISelect<Func<_, T>, ITable<_, T>> _tables;

	public ReferenceContext(ISelect<_, T> subject) : this(subject, ReferenceTables<_, T>.Default) {}

	public ReferenceContext(ISelect<_, T> subject, ISelect<Func<_, T>, ITable<_, T>> tables)
	{
		_subject = subject;
		_tables  = tables;
	}

	public ITable<_, T> New() => _tables.Get(_subject.Get);
}