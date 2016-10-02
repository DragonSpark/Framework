using System.Reflection;
using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework.Runtime
{
	public class MethodContext : Scope<MethodBase>
	{
		public static IScope<MethodBase> Default { get; } = new MethodContext();
		MethodContext() {}
	}
}