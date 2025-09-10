using DragonSpark.Model.Operations;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;

public interface IStopHandle : IOperation
{
	CancellationToken Token { get; }
}