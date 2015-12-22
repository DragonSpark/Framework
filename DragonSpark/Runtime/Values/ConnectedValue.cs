using Nito.ConnectedProperties;
using System;

namespace DragonSpark.Runtime.Values
{
	public abstract class ConnectedValue<T> : WritableValue<T>
	{
		/*public static T Property<TProperty>( [Required]object @this ) where TProperty : ConnectedValue<T>
		{
			var value = (ConnectedValue<T>)typeof(TProperty).GetConstructor( typeof(object) ).Invoke( new[] { @this } );
			var result = value.Item;
			return result;
		}*/

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
}