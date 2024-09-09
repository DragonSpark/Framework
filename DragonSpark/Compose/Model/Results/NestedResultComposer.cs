using DragonSpark.Model.Results;

namespace DragonSpark.Compose.Model.Results;

public class NestedResultComposer<T> : ResultComposer<IResult<T>>
{
	public NestedResultComposer(IResult<IResult<T>> subject) : base(subject) {}

	public ResultComposer<T> Assume() => new Assume<T>(Delegate()).Then();

	public ResultComposer<T> Value() => Select(Results<T>.Default);

	public ResultDelegateComposer<T> Delegate() => Select(DelegateSelector<T>.Default).Get().Then();
}