using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC.Build
{
	public class DefaultValueStrategy : BuilderStrategy
	{
		public override void PreBuildUp(IBuilderContext context)
		{
			typeof(IDefaultValueProvider).GetTypeInfo().IsAssignableFrom( context.BuildKey.Type.GetTypeInfo() ).IsFalse( () =>
			{
				var item = context.NewBuildUp<IDefaultValueProvider>();
				context.Existing = context.Existing.With( item.Apply );
			});
		}
	}
}