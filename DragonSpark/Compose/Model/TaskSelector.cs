using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public class TaskSelector<_, T> : Selector<_, Task<T>>
	{
		public TaskSelector(ISelect<_, Task<T>> subject) : base(subject) {}

		public OperationResultSelector<_, T> Promote()
			=> new OperationResultSelector<_, T>(Get().Select(SelectOperation<T>.Default));

		public TaskSelector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public TaskSelector<_, TTo> Select<TTo>(Func<T, TTo> select)
			=> new TaskSelector<_, TTo>(Get().Select(new Selection<T, TTo>(select)));
	}
}