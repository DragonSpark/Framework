using DragonSpark.Sources.Scopes;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Runtime
{
	public class MethodContext : Scope<MethodBase>
	{
		public static IScope<MethodBase> Default { get; } = new MethodContext();
		MethodContext() {}
	}
}