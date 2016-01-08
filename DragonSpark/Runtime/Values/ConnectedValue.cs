using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using Nito.ConnectedProperties;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Threading;

namespace DragonSpark.Runtime.Values
{
	public static class Ambient
	{
		public static T GetCurrent<T>() => new ThreadAmbientValue<T>().Item.PeekOrDefault();

		public static T[] GetCurrentChain<T>( [Required]this T @this ) => @this.Append( new ThreadAmbientValue<T>().Item.ToArray() ).Distinct().Fixed();

		public static T[] GetCurrentChain<T>() => new ThreadAmbientValue<T>().Item.ToArray();
	}

	public class ThreadLocalValue<T> : WritableValue<T>, IDisposable
	{
		readonly ThreadLocal<T> local;

		public ThreadLocalValue( [Required]Func<T> create ) : this( new ThreadLocal<T>( create ) ) {}

		public ThreadLocalValue( ThreadLocal<T> local )
		{
			this.local = local;
		}

		public override void Assign( T item ) => local.Value = item;

		public override T Item => local.Value;

		public void Dispose() => local.Dispose();
	}


	public abstract class ConnectedValue<T> : WritableValue<T>, IDisposable
	{
		readonly Func<T> create;

		protected ConnectedValue( object instance, Type type, Func<T> create = null ) : this( instance, type.AssemblyQualifiedName, create )
		{}

		protected ConnectedValue( [Required]object instance, [NotEmpty]string name, Func<T> create = null ) : this( PropertyConnector.Default.Get( instance, name, true ).Cast<T>(), create )
		{}

		protected ConnectedValue( [Required]ConnectibleProperty<T> property, Func<T> create )
		{
			Property = property;
			this.create = create ?? ( () => default(T) );
		}

		public override void Assign( T item ) => Property.Set( item );

		public override T Item => Property.GetOrCreate( create );

		public ConnectibleProperty<T> Property { get; }

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing ) => disposing.IsTrue( OnDispose );

		protected virtual void OnDispose()
		{
			Item.TryDispose();
			Property.TryDisconnect();
		}
	}

	public class Reference<T> : ConnectedValue<T>
	{
		public Reference( T key, object instance ) : base( instance, Key( key ), () => key )
		{ }

		public static string Key( object item ) => $"{typeof( T )}-{item.GetHashCode()}";
	}

	
	public class AssociatedValue<T> : AssociatedValue<object, T>
	{
		public AssociatedValue( object instance, Func<T> create = null ) : base( instance, create )
		{}
	}

	public class AssociatedValue<T, U> : ConnectedValue<U>
	{
		public AssociatedValue( T instance, Func<U> create = null ) : base( instance, typeof(AssociatedValue<T, U>), create )
		{ }
	}

	public class Items : Items<object>
	{
		public Items( object instance ) : base( instance )
		{ }
	}

	public class Items<T> : ConnectedValue<IList<T>>
	{
		public Items( object instance ) : base( instance, typeof(Items<T>), () => new List<T>() )
		{ }
	}
}