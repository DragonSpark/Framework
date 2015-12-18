using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Activation.IoC
{
	public interface IObjectBuilderPolicy : IBuilderPolicy
	{
		bool Enabled { get; }
	}

	class ObjectBuilderPolicy : IObjectBuilderPolicy
	{
		public ObjectBuilderPolicy( bool enabled )
		{
			Enabled = enabled;
		}

		public bool Enabled { get; }
	}

	public class ObjectBuilderStrategy : BuilderStrategy
	{
		public override void PostBuildUp( IBuilderContext context )
		{
			context.Policies.Get<IObjectBuilderPolicy>( context.BuildKey ).With( policy => policy.Enabled ).IsTrue( () =>
			{
				var item = context.NewBuildUp<IObjectBuilder>();
				context.Existing = context.Existing.With( item.BuildUp );
			} );
		}
	}
}