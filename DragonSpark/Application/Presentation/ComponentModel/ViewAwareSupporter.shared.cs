using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public class ViewAwareSupporter : IAttachedObject
    {
        readonly IAttachedObject owner;
        public event EventHandler Attached = delegate {}, Detached = delegate {};

        public ViewAwareSupporter( IAttachedObject owner )
        {
            this.owner = owner;
        }

        public IEnumerable<IAttachedObject> Items
        {
            get { return items ?? ( items = ResolveItems() ); }
        }	IEnumerable<IAttachedObject> items;

        IEnumerable<IAttachedObject> ResolveItems()
        {
            var result = owner.GetAllPropertyValuesOf<IAttachedObject>().Concat( owner.GetAllPropertyValuesOf<IDictionary>().SelectMany( x => x.Keys.Cast<object>().Select( y => x[y].As<IAttachedObject>() ) ) ).NotNull().ToArray();
            return result;
        }

        public void Attach( DependencyObject dependencyObject )
        {
            AssociatedObject = dependencyObject;
            Attached( owner, EventArgs.Empty );
            Items.Apply( x => x.Attach( dependencyObject ) );
        }

        public void Detach()
        {
            Detached( owner, EventArgs.Empty );
            Items.Apply( x => x.Detach() );
        }

        public DependencyObject AssociatedObject { get; private set; }
    }
}