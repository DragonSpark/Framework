using System.Windows;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class EnsureVisualStateGroup : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			var groups = VisualStateManager.GetVisualStateGroups( AssociatedObject );
			switch ( groups.Count )
			{
				case 0:
					groups.Add( new VisualStateGroup() );
					break;
			}
			
			base.OnAttached();
		}
	}
}