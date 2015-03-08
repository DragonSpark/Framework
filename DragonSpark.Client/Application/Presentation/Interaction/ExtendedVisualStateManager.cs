using System;
using System.Windows;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ExtendedVisualStateManager : Microsoft.Expression.Interactivity.Core.ExtendedVisualStateManager
	{
		protected override bool GoToStateCore( System.Windows.Controls.Control control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, System.Windows.VisualState state, bool useTransitions )
		{
			try
			{
				// control.NotNull( x => x.ApplyTemplate() );
				var result = base.GoToStateCore( control, stateGroupsRoot, stateName, group, state, useTransitions );
				return result;
			}
			catch ( InvalidOperationException error )
			{
				DragonSpark.Runtime.Logging.Error( error );
				return false;
			}
		}
	}
}