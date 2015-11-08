using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC
{
	public class DefaultValueStrategy : BuilderStrategy
	{
		public override void PreBuildUp(IBuilderContext context)
		{
			typeof(IDefaultValueProvider).IsAssignableFrom( context.BuildKey.Type ).IsFalse( () =>
			{
				var item = context.NewBuildUp<IDefaultValueProvider>();
				context.Existing = context.Existing.With( item.Apply );
			});
		}
	}
}