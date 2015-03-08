using System;

namespace DragonSpark.Application.Communication
{
	[AttributeUsage( AttributeTargets.Class )]
	public sealed class WcfBehaviorBaseWcfSilverlightFaultBehaviorAttribute : WcfBehaviorBaseAttribute
	{
		public WcfBehaviorBaseWcfSilverlightFaultBehaviorAttribute()
			: base(typeof(WcfSilverlightFaultBehavior))
		{
		}
	}
}