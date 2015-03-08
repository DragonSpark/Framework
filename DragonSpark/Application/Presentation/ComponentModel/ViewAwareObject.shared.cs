using System;
using System.Windows;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public class ViewAwareObject : ViewObject, IAttachedObject
    {
        readonly ViewAwareSupporter viewAwareSupporter;

        protected ViewAwareObject()
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
        {}

        void System.Windows.Interactivity.IAttachedObject.Detach()
        {
            OnDetached();
        }

        protected virtual void OnDetached()
        {}

        public DependencyObject AssociatedObject
        {
            get { return viewAwareSupporter.AssociatedObject; }
        }
        #endregion
    }
}