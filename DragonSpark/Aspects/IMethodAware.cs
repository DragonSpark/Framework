using System.Reflection;

namespace DragonSpark.Aspects
{
	public interface IMethodAware
	{
		MethodInfo Method { get; }
	}
}