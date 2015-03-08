using System;

namespace DragonSpark.Application.Communication
{
    [AttributeUsage( AttributeTargets.Class )]
    public sealed class WcfBehaviorBaseWcfErrorBehaviorAttribute : WcfBehaviorBaseAttribute
    {
        public WcfBehaviorBaseWcfErrorBehaviorAttribute()
            : base( typeof(WcfErrorBehavior) )
        {}
    }
}