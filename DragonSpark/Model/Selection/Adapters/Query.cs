using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Model.Sequences.Query.Construction;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Model.Selection.Adapters
{
	public class Query<_, T> : IResult<ISelect<_, Array<T>>>
	{
		readonly INode<_, T> _node;

		public Query(ISelect<_, T[]> subject) : this(new StartNode<_, T>(subject)) {}

		public Query(ISelect<_, Sequences.Store<T>> subject) : this(new Node<_, T>(subject)) {}

		public Query(INode<_, T> node) => _node = node;

		public ISelect<_, Array<T>> Get() => Out().Select(Sequences.Result<T>.Default);

		public Query<_, T> Select(IPartition partition) => new Query<_, T>(_node.Get(partition));

		public Query<_, T> Select(IBodyBuilder<T> builder) => new Query<_, T>(_node.Get(builder));

		public Query<_, TOut> Select<TOut>(IContents<T, TOut> select) => new Query<_, TOut>(_node.Get(select));

		public ISelect<_, TTo> Select<TTo>(IReduce<T, TTo> select) => _node.Get(select);

		public ISelect<_, T[]> Out() => _node.Get();
	}

	public sealed class SelectQueryContext<_, T>
	{
		readonly Query<_, T> _subject;

		public SelectQueryContext(Query<_, T> query) => _subject = query;

		public Query<_, TOut> By<TOut>(Expression<Func<T, TOut>> select)
			=> _subject.Select(new Build.InlineSelect<T, TOut>(select).Returned());

		public Query<_, TOut> Many<TOut>(Expression<Func<T, IEnumerable<TOut>>> select)
			=> _subject.SelectMany(select.Compile());
	}

	public sealed class WhereQueryContext<_, T>
	{
		readonly Query<_, T> _subject;

		public WhereQueryContext(Query<_, T> query) => _subject = query;

		public Query<_, T> By(Expression<Func<T, bool>> where) => _subject.Where(where.Compile());
	}
}