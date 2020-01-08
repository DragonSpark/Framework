using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model
{
	public class ResultSelectionSelector<_, T> : Selector<_, IResult<T>>
	{
		public ResultSelectionSelector(ISelect<_, IResult<T>> subject) : base(subject) {}

		public Selector<_, T> Value() => Select(Results<T>.Default);

		public Selector<_, Func<T>> Delegate() => Select(DelegateSelector<T>.Default);
	}

	public class ResultDelegateContext<T> : ResultContext<Func<T>>
	{
		public ResultDelegateContext(IResult<Func<T>> instance) : base(instance) {}

		public ResultContext<T> Assume() => new Assume<T>(this).Then();
	}

	public class CommandResultContext<T> : ResultContext<ICommand<T>>
	{
		public CommandResultContext(IResult<ICommand<T>> instance) : base(instance) {}

		public CommandSelector<T> Assume() => new DelegatedInstanceCommand<T>(Get()).Then();
	}

	public class NestedResultContext<T> : ResultContext<IResult<T>>
	{
		public NestedResultContext(IResult<IResult<T>> subject) : base(subject) {}

		public ResultContext<T> Assume() => new Assume<T>(Delegate()).Then();

		public ResultContext<T> Value() => Select(Results<T>.Default);

		public ResultDelegateContext<T> Delegate() => Select(DelegateSelector<T>.Default).Get().Then();
	}
}