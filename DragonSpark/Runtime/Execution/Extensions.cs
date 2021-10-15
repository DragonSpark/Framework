using DragonSpark.Compose;

namespace DragonSpark.Runtime.Execution;

public static class Extensions
{
	public static int Count(this ICounter @this) => @this.Parameter().Get();
}