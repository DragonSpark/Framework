using System.Security.Principal;
using System.Windows;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class RefreshBindingsBehavior : PrincipalRefreshedBehaviorBase
	{
		protected override void OnRefresh( IPrincipal principle )
		{
			AssociatedObject.GetAllChildren<FrameworkElement>().Apply( y => FrameworkElementExtensions.GetAllBindings( y ).Apply( z =>
			{
				y.ClearValue( z.Item1 );
				y.SetBinding( z.Item1, z.Item2.ParentBinding );
			} ) );
		}
	}
}