using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Launch
{
	public abstract class ApplicationLaunchEventConfiguration : UnityContainerConfiguration
	{
		public ApplicationLaunchStatus TargetStatus { get; set; }

		protected override void Configure( IUnityContainer container )
		{
			var aggregator = container.Resolve<IEventAggregator>();
			aggregator.ExecuteWhenStatusIs( TargetStatus, () => base.Configure( container ) );
		}
	}
}