using System.Reflection;
using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework
{
	public sealed class CurrentTestingMethod : Scope<MethodBase>
	{
		public static CurrentTestingMethod Default { get; } = new CurrentTestingMethod();
		CurrentTestingMethod() {}
	}
}