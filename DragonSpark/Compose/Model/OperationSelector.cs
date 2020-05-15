using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public class OperationSelector<_, T> : Selector<_, ValueTask<T>>
	{
		public OperationSelector(ISelect<_, ValueTask<T>> subject) : base(subject) {}

		public TaskSelector<_, T> Demote() => new TaskSelector<_, T>(Get().Select(SelectTask<T>.Default));

		public OperationSelector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public OperationSelector<_, TTo> Select<TTo>(Func<T, TTo> select)
			=> new OperationSelector<_, TTo>(Get().Select(new OperationSelect<T, TTo>(select)));
	}
}