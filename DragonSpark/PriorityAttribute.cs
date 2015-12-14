using System;

namespace DragonSpark
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property )]
	public sealed class PriorityAttribute : Attribute, IAllowsPriority
	{
		public PriorityAttribute( Priority priority )
		{
			Priority = priority;
		}

		public Priority Priority { get; }
	}
}