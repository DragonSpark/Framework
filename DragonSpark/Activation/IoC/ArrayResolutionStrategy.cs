using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC
{
	public class ArrayResolutionStrategy : Microsoft.Practices.Unity.ArrayResolutionStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			if ( !context.HasBuildPlan() )
			{
				base.PreBuildUp( context );
			}
		}
	}
}