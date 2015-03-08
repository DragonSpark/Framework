using System.Linq;
using System.Windows;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public class RefreshOnPrincipalChangedBehavior : RegionBehaviorBase<FrameworkElement>
    {
        readonly INotifyPrincipalChanged principal;

        public RefreshOnPrincipalChangedBehavior( INotifyPrincipalChanged principal )
        {
            this.principal = principal;
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            principal.PrincipalChanged += PrincipalOnPrincipalChanged;
        }

        void PrincipalOnPrincipalChanged( object sender, PrincipalChangedEventArgs principalChangedEventArgs )
        {
            Region.Views.OfType<IViewObject>().Apply( x => x.RefreshAllNotifications() );
        }
    }
}