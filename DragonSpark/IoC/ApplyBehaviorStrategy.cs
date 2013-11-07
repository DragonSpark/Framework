using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using System.Configuration;
using System.Linq;

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

	public class ApplicationConfigurationStrategy : BuilderStrategy
	{
		public override void PreBuildUp( IBuilderContext context )
		{
			if ( context.BuildKey.Type == typeof(string) && context.Existing == null && !context.BuildComplete )
			{
				var strategy = context.Strategies.FirstOrDefaultOfType<CurrentContextStrategy>();
				var operation = strategy.Stack.Select( x => x.CurrentOperation ).OfType<ConstructorArgumentResolveOperation>().LastOrDefault();
				context.Existing = operation.Transform( x => ConfigurationManager.AppSettings.Get( x.ParameterName ) != null ? ConfigurationManager.AppSettings[x.ParameterName] : null );
				context.BuildComplete = context.Existing != null;
			}

			base.PreBuildUp( context );
		}
	}
}