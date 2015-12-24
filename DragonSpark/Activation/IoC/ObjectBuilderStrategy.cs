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
		public override void PreBuildUp( IBuilderContext context )
		{
			var all = context.GetBuildChain().All( key => context.Policies.Get<IObjectBuilderPolicy>( key ).With( policy => policy.Enabled ) );
			all.IsTrue( () =>
			{
				var item = context.NewBuildUp<IObjectBuilder>();
				context.Existing = context.Existing.With( item.BuildUp );
			} );
		}
	}
}