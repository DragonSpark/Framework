using System.Security.Principal;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.IoC;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Interaction
{
	public abstract class PrincipalRefreshedBehaviorBase : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			this.BuildUpOnce();

			PrincipalChanged.PrincipalChanged += PrincipalChangedOnPrincipalChanged;

			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			PrincipalChanged.PrincipalChanged -= PrincipalChangedOnPrincipalChanged;
			base.OnDetaching();
		}

		void PrincipalChangedOnPrincipalChanged( object sender, PrincipalChangedEventArgs principalChangedEventArgs )
		{
			OnRefresh( PrincipalChanged );
		}

		protected abstract void OnRefresh( IPrincipal principle );

		[Dependency]
		public INotifyPrincipalChanged PrincipalChanged { get; set; }
	}
}