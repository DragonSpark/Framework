using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class DataFormCancelCompensations : Behavior<DataForm>
	{
		protected override void OnAttached()
		{
			AssociatedObject.EditEnding += AssociatedObjectEditEnding;
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			AssociatedObject.EditEnding -= AssociatedObjectEditEnding;
			base.OnDetaching();
		}

		void AssociatedObjectEditEnding( object sender, DataFormEditEndingEventArgs e )
		{
			switch ( AssociatedObject.Mode )
			{
				case DataFormMode.AddNew:
					switch ( e.EditAction )
					{
						case DataFormEditAction.Cancel:
							AssociatedObject.EditEnding -= AssociatedObjectEditEnding;
							try
							{
								AssociatedObject.CancelEdit();
							}
							catch ( InvalidOperationException )
							{}
							finally
							{
								AssociatedObject.EditEnding += AssociatedObjectEditEnding;
							}
							break;
					}
					break;
			}
		}
	}
}