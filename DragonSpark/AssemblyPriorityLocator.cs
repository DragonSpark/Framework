using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark
{
	public sealed class AssemblyPriorityLocator : PriorityAwareLocator<Assembly>
	{
		public new static AssemblyPriorityLocator Default { get; } = new AssemblyPriorityLocator();
		AssemblyPriorityLocator() : base( assembly => assembly.GetAttribute<PriorityAttribute>() ) {}
	}
}