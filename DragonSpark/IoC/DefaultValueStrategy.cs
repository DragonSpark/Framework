using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC
{
	public class DefaultValueStrategy : BuilderStrategy
	{
		public override void PreBuildUp(IBuilderContext context)
		{
			context.Existing = context.Existing.Transform( item => item.WithDefaults() );
		}
	}
}