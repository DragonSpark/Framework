using DragonSpark.Model.Operations;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;

public interface ITokenHandle : IOperation
{
	CancellationToken Token { get; }
}