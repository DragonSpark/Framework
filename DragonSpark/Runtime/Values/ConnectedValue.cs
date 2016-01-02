using Nito.ConnectedProperties;
using System;
using System.Collections.Generic;

namespace DragonSpark.Runtime.Values
{
	public abstract class ConnectedValue<T> : WritableValue<T>
	{
		readonly Func<T> create;

		protected ConnectedValue( object instance, Type type, Func<T> create = null ) : this( instance, type.AssemblyQualifiedName, create )
		{}

		protected ConnectedValue( object instance, string name, Func<T> create = null ) : this( PropertyConnector.Default.Get( instance, name, true ).Cast<T>(), create )
		{}

		protected ConnectedValue( ConnectibleProperty<T> property, Func<T> create )
		{
			Property = property;
			this.create = create ?? ( () => default(T) );
		}

		public override void Assign( T item )
		{
			Property.Set( item );
		}

		public override T Item => Property.GetOrCreate( create );

		public ConnectibleProperty<T> Property { get; }
	}

	public class AssociatedValue<T, U> : ConnectedValue<U>
	{
		public AssociatedValue( T instance ) : base( instance, typeof( AssociatedValue<T, U> ) )
		{ }
	}

	public class Items : Items<object>
	{
		public Items( object instance ) : base( instance )
		{ }
	}

	public class Items<T> : ConnectedValue<IList<T>>
	{
		public Items( object instance ) : base( instance, typeof( Items<T> ), () => new List<T>() )
		{ }
	}
}