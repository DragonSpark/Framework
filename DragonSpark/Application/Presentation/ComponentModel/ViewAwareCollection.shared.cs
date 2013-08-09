using System;
using System.Linq;
using System.Windows;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public class ViewAwareCollection<T> : ViewCollection<T>, IAttachedObject
    {
        readonly ViewAwareSupporter viewAwareSupporter;

        public ViewAwareCollection()
        {
            viewAwareSupporter = new ViewAwareSupporter( this );
        }

        #region IAttachedObject
        public event EventHandler Attached
        {
            add { viewAwareSupporter.Attached += value; }
            remove { viewAwareSupporter.Attached -= value; }
        }

        public event EventHandler Detached
        {
            add { viewAwareSupporter.Detached += value; }
            remove { viewAwareSupporter.Detached -= value; }
        }

        void System.Windows.Interactivity.IAttachedObject.Attach( DependencyObject dependencyObject )
        {
            viewAwareSupporter.Attach( dependencyObject );
            OnAttached();
        }

        protected virtual void OnAttached()
        {
            this.OfType<IAttachedObject>().Apply( x => x.Attach( AssociatedObject ) );
        }

        void System.Windows.Interactivity.IAttachedObject.Detach()
        {
            OnDetached();
        }

        protected virtual void OnDetached()
        {
            this.OfType<IAttachedObject>().Apply( x => x.Detach() );
        }

        public DependencyObject AssociatedObject
        {
            get { return viewAwareSupporter.AssociatedObject; }
        }
        #endregion
    }
}