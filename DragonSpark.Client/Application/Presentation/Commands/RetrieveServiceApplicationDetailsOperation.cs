using DragonSpark.Objects;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Commands
{
	public class RetrieveServiceApplicationDetailsOperation : OperationCommandBase
	{
		readonly IUnityContainer container;
		readonly IApplicationService applicationService;
		readonly string registrationName;

		public RetrieveServiceApplicationDetailsOperation( IUnityContainer container, IApplicationService applicationService, string registrationName = "ServiceDetails" )
		{
			this.container = container;
			this.applicationService = applicationService;
			this.registrationName = registrationName;
		}

		protected override void ExecuteCommand( ICommandMonitor commandMonitor )
		{
			var details = applicationService.RetrieveApplicationDetails();
			container.RegisterInstance( registrationName, details );
		}

		[DefaultPropertyValue( "Retrieving Application Details from Service." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}

	/*public sealed class NavigationService : ViewObject, System.Windows.IApplicationService
	{
		[Dependency]
		public INavigationNodeMap NodeMap { get; set; }

		public void StartService( ApplicationServiceContext context )
		{
			System.Windows.Application.Current.Host.NavigationStateChanged += HostNavigationStateChanged;
			Update();
		}

		void HostNavigationStateChanged( object sender, NavigationStateChangedEventArgs e )
		{
			Update();
		}

		void Update()
		{
			Deployment.Current.Dispatcher.BeginInvoke( () =>
			{
			    Exists = NodeMap.Exists( new Uri( System.Windows.Application.Current.Host.NavigationState, UriKind.Relative ) );
			} );
		}

		public bool Exists
		{
			get { return exists; }
			private set
			{
				if ( exists != value )
				{
					exists = value;
					NotifyOfPropertyChange( () => Exists );
				}
			}
		}	bool exists;

		void System.Windows.IApplicationService.StopService()
		{
			System.Windows.Application.Current.Host.NavigationStateChanged -= HostNavigationStateChanged;
		}
	}*/

	/*public sealed class ApplicationDetailsService : ViewObject, System.Windows.IApplicationService
	{
		[Dependency]
		public IApplicationService ApplicationService { get; set; }

		[Dependency]
		public ApplicationDetails ClientDetails { get; set; }

		public ApplicationDetails ServiceDetails
		{
			get { return serviceDetails; }
			private set { SetProperty( ref serviceDetails, value, () => ServiceDetails ); }
		}	ApplicationDetails serviceDetails;

		/*IEnumerable<object> IApplicationInitializer.Initialize()
		{
			yield return new Operation( "Retrieving Service Details", () =>
			{
				ServiceDetails = ApplicationService.RetrieveApplicationDetails();
			} ) { Mode = OperationMode.Background };
			yield break;
		}#1#

		public int Order
		{
			get { return 0; }
		}

		void System.Windows.IApplicationService.StartService( ApplicationServiceContext context )
		{}

		void System.Windows.IApplicationService.StopService()
		{}
	}*/
}
