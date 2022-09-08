using DragonSpark.Runtime.Execution;
using System.Threading;

namespace DragonSpark.Model.Operations;

public sealed class AmbientToken : Logical<CancellationToken>
{
	public static AmbientToken Default { get; } = new();

	AmbientToken() {}
}