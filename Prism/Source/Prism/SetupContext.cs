using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Prism
{
    public class SetupContext
    {
        readonly object arguments;
        readonly bool useDefaultConfiguration;
        
        public SetupContext( object arguments, bool useDefaultConfiguration )
        {
            this.arguments = arguments;
            this.useDefaultConfiguration = useDefaultConfiguration;
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
                throw new InvalidOperationException( string.Format( "Arguments are not of type '{0}'", typeof(T).Name ) );
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

        public ILoggerFacade Logger
        {
            get { return Item<ILoggerFacade>(); }
        }

        public bool UseDefaultConfiguration
        {
            get { return useDefaultConfiguration; }
        }
    }
}