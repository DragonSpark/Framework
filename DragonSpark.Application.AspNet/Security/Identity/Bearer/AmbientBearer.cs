using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class AmbientBearer : Logical<string>
{
	public static AmbientBearer Default { get; } = new();

	AmbientBearer() {}
}