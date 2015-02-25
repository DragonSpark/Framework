using System;

namespace DragonSpark
{
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property )]
	public sealed class PriorityAttribute : Attribute, IAllowsPriority
	{
		readonly Priority priority;

		public PriorityAttribute( Priority priority )
		{
			this.priority = priority;
		}

		public Priority Priority
		{
			get { return priority; }
		}
	}
}