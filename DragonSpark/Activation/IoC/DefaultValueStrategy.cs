using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC
{
	public class DefaultValueStrategy : BuilderStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			var execute = !typeof(IDefaultValueProvider).IsAssignableFrom( context.BuildKey.Type ) && context.IsRegistered<IDefaultValueProvider>();
			execute.IsTrue( () =>
			{
				var item = context.NewBuildUp<IDefaultValueProvider>();
				context.Existing = context.Existing.With( item.Apply );
			});
		}
	}
}