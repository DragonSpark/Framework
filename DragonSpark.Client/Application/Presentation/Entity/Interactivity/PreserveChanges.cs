using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public class PreserveChanges : Behavior<DataForm>
	{
		Frame Frame { get; set; }

		protected override void OnAttached()
		{
			AssociatedObject.EnsureLoaded( x =>
			{
			    Frame = x.GetRootOfType<Frame>();
			    Frame.NotNull( y => y.Navigating += NavigationServiceNavigating );
			} );
			base.OnAttached();
		}

		protected override void OnDetaching()
		{
			Frame.NotNull( x => x.Navigating -= NavigationServiceNavigating );
			base.OnDetaching();
		}

		void NavigationServiceNavigating( object sender, NavigatingCancelEventArgs e )
		{
			e.Cancel.IsFalse( () =>
			{
			    AssociatedObject.CurrentItem.As<IRevertibleChangeTracking>( x =>
				{
					var dialogResult = System.Windows.Interactivity.Interaction.GetBehaviors( AssociatedObject ).FirstOrDefaultOfType<IPreserveChangesDialog>().Transform( y => y.CanDismissChanges( x ) );
					dialogResult.NotNull( y =>
					{
						e.Cancel = !y.Value;
						if ( !e.Cancel )
						{
							AssociatedObject.CancelEdit();
						}
					} );
				} );
			} );
		}
	}
}