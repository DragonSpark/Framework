using DragonSpark.Model.Results;

namespace DragonSpark.Application.Model.Interaction;

public sealed class SuccessResult : SuccessResultBase
{
	public static SuccessResult Default { get; } = new();

	SuccessResult() {}
}

public class SuccessResult<T> : SuccessResultBase, IResult<T>
{
	public SuccessResult(T instance) => Instance = instance;

	public T Instance { get; }

	public T Get() => Instance;
}