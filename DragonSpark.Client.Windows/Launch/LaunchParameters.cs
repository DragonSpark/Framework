using System.Threading;
using System.Windows;
using System.Windows.Markup;
using DragonSpark.Client.Windows.Commanding;
using DragonSpark.Client.Windows.Extensions;
using DragonSpark.Common.IoC.Commands;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;

namespace DragonSpark.Client.Windows.Launch
{

	[ContentProperty( "Shell" )]
	public class LaunchParameters
	{
		public LaunchParameters()
		{
			RunWithDefaultConfiguration = true;
		}

		public Window Shell { get; set; }

		public IModuleCatalog Catalog { get; set; }

		public bool RunWithDefaultConfiguration { get; set; }

		public bool LaunchShellAsDialog { get; set; }

		public string[] Arguments { get; set; }
	}

	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class LaunchInitializationOperation : CommandBase<IEventAggregator>
	{
		readonly ManualResetEvent initialized = new ManualResetEvent( false );

		protected override void Execute( IEventAggregator parameter )
		{
			parameter.ExecuteWhenStatusIs( ApplicationLaunchStatus.Initialized, () => initialized.Set() )
				.IsFalse( () => initialized.WaitOne() );
		}
	}
}