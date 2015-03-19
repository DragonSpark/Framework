using System.Windows;
using System.Windows.Markup;
using Prism.Modularity;

namespace DragonSpark.Application.Client
{

	[ContentProperty( "Shell" )]
	public class SetupParameters
	{
		public SetupParameters()
		{
			RunWithDefaultConfiguration = true;
		}

		/*public Window Shell { get; set; }

		public IModuleCatalog Catalog { get; set; }*/

		public bool RunWithDefaultConfiguration { get; set; }

		/*public bool LaunchShellAsDialog { get; set; }*/

		public string[] Arguments { get; set; }
	}

	/*[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class LaunchInitializationOperation : CommandBase<IEventAggregator>
	{
		readonly ManualResetEvent initialized = new ManualResetEvent( false );

		protected override void Execute( IEventAggregator parameter )
		{
			parameter.ExecuteWhenStatusIs( ApplicationLaunchStatus.Initialized, () => initialized.Set() )
				.IsFalse( () => initialized.WaitOne() );
		}
	}*/
}