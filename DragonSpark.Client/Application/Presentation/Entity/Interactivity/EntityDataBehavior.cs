using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public class EntityDataBehavior : Behavior<DataForm>
    {
        protected override void OnAttached()
        {
            AssociatedObject.BeginningEdit += AssociatedObjectOnBeginningEdit;
            AssociatedObject.EditEnded += AssociatedObjectOnEditEnded;
            base.OnAttached();
            switch ( AssociatedObject.Mode )
            {
                case DataFormMode.Edit:
                    AttachBehaviors();
                    break;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.BeginningEdit -= AssociatedObjectOnBeginningEdit;
            AssociatedObject.EditEnded -= AssociatedObjectOnEditEnded;
            base.OnDetaching();
        }

        void AssociatedObjectOnBeginningEdit( object sender, CancelEventArgs cancelEventArgs )
        {
            AttachBehaviors();
        }

        void AttachBehaviors()
        {
            var items = Behaviors ?? Enumerable.Empty<IEntityDataBehavior>();
            items.Apply( x => x.Attach( AssociatedObject.CurrentItem ) );
        }

        void AssociatedObjectOnEditEnded( object sender, DataFormEditEndedEventArgs dataFormEditEndedEventArgs )
        {
            var items = Behaviors ?? Enumerable.Empty<IEntityDataBehavior>();
            items.Apply( x => x.Detach() );
        }

        public IEnumerable<IEntityDataBehavior> Behaviors
        {
            get { return GetValue( BehaviorsProperty ).To<IEnumerable<IEntityDataBehavior>>(); }
            set { SetValue( BehaviorsProperty, value ); }
        }	public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.Register( "Behaviors", typeof(IEnumerable<IEntityDataBehavior>), typeof(EntityDataBehavior), null );
    }
}