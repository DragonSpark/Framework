using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC
{
	public class ApplyBehaviorStrategy : BuilderStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			var policy = context.Policies.Get<IBehaviorPolicy>( context.BuildKey );
			policy.NotNull( x => x.Apply( context.Existing ) );
		}
	}
}