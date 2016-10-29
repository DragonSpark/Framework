using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Reflection;

namespace DragonSpark
{
	public sealed class AssemblyPriorityLocator : PriorityAwareLocator<Assembly>
	{
		public new static AssemblyPriorityLocator Default { get; } = new AssemblyPriorityLocator();
		AssemblyPriorityLocator() : base( assembly => assembly.GetAttribute<PriorityAttribute>() ) {}

		public IPriorityAware Get( Type type ) => Get( type.Assembly() );
	}
}