using System.Windows.Controls;
using DragonSpark.Application.Presentation.Infrastructure;

namespace DragonSpark.Application.Presentation.Navigation
{
    public class AuthenticationRefreshFrameBehavior : RegionBehaviorBase<Frame>
    {
        readonly INotifyPrincipalChanged principal;

        public AuthenticationRefreshFrameBehavior( INotifyPrincipalChanged principal )
        {
            this.principal = principal;
        }

        protected override void OnAttach()
        {
            principal.PrincipalChanged += ( s, a ) => AssociatedControl.Refresh();
            base.OnAttach();
        }
    }
}