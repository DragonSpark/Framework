using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Runtime.Operations;

public interface IStopHandle : IOperation
{
	CancellationToken Token { get; }
}