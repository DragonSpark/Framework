using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public class ConfirmRemoval : Behavior<DataForm>
    {
        public event EventHandler Removed = delegate { };
		
        protected override void OnAttached()
        {
            AssociatedObject.DeletingItem += AssociatedObjectDeletingItem;
			
            base.OnAttached();
        }

        void AssociatedObjectDeletingItem(object sender, CancelEventArgs e)
        {
            AssociatedObject.CurrentItem.As<IRevertibleChangeTracking>( x =>
            {
                var dialogResult = System.Windows.Interactivity.Interaction.GetBehaviors( AssociatedObject ).FirstOrDefaultOfType<IRemovalDialog>().Transform( y => y.CanRemove( x ) );
                dialogResult.NotNull( y =>
                {
                    e.Cancel = !y.Value;
                    if ( !e.Cancel )
                    {
                        Dispatcher.BeginInvoke( () => Removed( this, EventArgs.Empty ) );
                    }
                } );
            } );
        }
    }
}