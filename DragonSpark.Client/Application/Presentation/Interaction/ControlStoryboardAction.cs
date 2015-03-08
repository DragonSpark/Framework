using System.Windows.Media.Animation;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ControlStoryboardAction : Microsoft.Expression.Interactivity.Media.ControlStoryboardAction
	{
		protected override void Invoke( object parameter )
		{
			Storyboard.NotNull( x =>
			{
				switch ( x.GetCurrentState() )
				{
					case ClockState.Stopped:
						Storyboard.SetTarget( x, AssociatedObject );
						break;
				}
			} );
			base.Invoke( parameter );
		}
	}
}