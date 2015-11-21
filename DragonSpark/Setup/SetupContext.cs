using DragonSpark.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Setup
{
    public class SetupContext
    {
        readonly object arguments;
        
        public SetupContext( object arguments )
        {
            this.arguments = arguments;
 	        Register( arguments );
        }

        public TItem Register<TItem>( TItem item )
        {
            items.Add( new WeakReference( item ) );
            return item;
        }

        public virtual TItem Item<TItem>()
        {
            var result = Items.OfType<TItem>().SingleOrDefault();
            return result;
        }

        IEnumerable<WeakReference> Update()
        {
            var remove = items.Where( reference => !reference.IsAlive );
            foreach ( var reference in remove )
            {
                items.Remove( reference );
            }
            return items;
        }

        public T GetArguments<T>()
        {
            if ( arguments == null )
            {
                throw new InvalidOperationException( "Arguments are not defined." );
            }

            if ( !typeof(T).GetTypeInfo().IsAssignableFrom( arguments.GetType().GetTypeInfo() ) )
            {
                throw new InvalidOperationException( $"Arguments are not of type '{typeof(T).Name}'" );
            }

            var result = (T)arguments;
            return result;
        }

        public IReadOnlyCollection<object> Items
        {
            get
            {
                var inner = Update().Select( reference => reference.Target ).ToList();
                return new ReadOnlyCollection<object>( inner );
            }
        }	readonly IList<WeakReference> items = new Collection<WeakReference>();

        public ILogger Logger => Item<ILogger>();
    }
}