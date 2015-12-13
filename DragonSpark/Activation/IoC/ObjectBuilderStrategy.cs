using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC
{
	public class ObjectBuilderStrategy : BuilderStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			var execute = !typeof(IObjectBuilder).Adapt().IsAssignableFrom( context.BuildKey.Type ) && context.IsRegistered<IObjectBuilder>();
			execute.IsTrue( () =>
			{
				var item = context.NewBuildUp<IObjectBuilder>();
				context.Existing = context.Existing.With( item.BuildUp );
			});
		}
	}
}