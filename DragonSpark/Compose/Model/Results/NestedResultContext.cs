using DragonSpark.Model.Results;

namespace DragonSpark.Compose.Model.Results
{
	public class NestedResultContext<T> : ResultContext<IResult<T>>
	{
		public NestedResultContext(IResult<IResult<T>> subject) : base(subject) {}

		public ResultContext<T> Assume() => new Assume<T>(Delegate()).Then();

		public ResultContext<T> Value() => Select(Results<T>.Default);

		public ResultDelegateContext<T> Delegate() => Select(DelegateSelector<T>.Default).Get().Then();
	}
}