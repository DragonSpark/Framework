using DragonSpark.Compose.Extents.Selections;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public class OperationSelector<_, T> : Selector<_, ValueTask<T>>
	{
		public static implicit operator Operate<_, T>(OperationSelector<_, T> instance) => instance.Get().Get;

		public static implicit operator Await<_, T>(OperationSelector<_, T> instance) => instance.Get().Await!; // ISSUE: NRT

		public OperationSelector(ISelect<_, ValueTask<T>> subject) : base(subject) {}

		public TaskSelector<_, T> Demote() => new TaskSelector<_, T>(Get().Select(SelectTask<T>.Default));

		public OperationSelector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public OperationSelector<_, TTo> Select<TTo>(ISelect<T, ValueTask<TTo>> select)
			=> Select(select.Get);

		public OperationSelector<_, TTo> Select<TTo>(Func<T, ValueTask<TTo>> select)
		{
			var subject = Get().Select(new OperationSelect<T, ValueTask<TTo>>(@select)).Select(Awaiting<TTo>.Default);
			var result  = new OperationSelector<_, TTo>(subject);
			return result;
		}

		public OperationSelector<_, TTo> Select<TTo>(Func<T, TTo> select)
			=> new OperationSelector<_, TTo>(Get().Select(new OperationSelect<T, TTo>(select)));


		public OperationContext<_> Terminate(ISelect<T, ValueTask> command) => Terminate(command.Get);
		public OperationContext<_> Terminate(Func<T, ValueTask> command)
			=> new OperationContext<_>(Get().Select(new OperationSelect<T>(command)));
	}
}