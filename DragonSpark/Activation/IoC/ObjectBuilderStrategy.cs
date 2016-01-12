using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
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
			var all = context.BuildKey.Append( Ambient.GetCurrentChain<NamedTypeBuildKey>() ).All( key => context.Policies.Get<IObjectBuilderPolicy>( key ).With( policy => policy.Enabled ) );
			all.IsTrue( () =>
			{
				var item = context.New<IObjectBuilder>();
				context.Existing = context.Existing.With( item.BuildUp );
			} );
		}
	}
}