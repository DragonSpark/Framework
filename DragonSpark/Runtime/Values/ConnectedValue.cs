using DragonSpark.Extensions;
using Nito.ConnectedProperties;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DragonSpark.Runtime.Values
{
	public static class Ambient
	{
		public static T GetCurrent<T>() => new ThreadAmbientChain<T>().Item.PeekOrDefault();

		// public static T[] GetCurrentChain<T>() => new ThreadAmbientChain<T>().Item.ToArray();

		public static T[] GetCurrentChain<T>() => new ThreadAmbientChain<T>().Item.ToArray();
	}

	public class ThreadLocalValue<T> : WritableValue<T>
	{
		readonly ThreadLocal<T> local;

		public ThreadLocalValue( [Required]Func<T> create ) : this( new ThreadLocal<T>( create ) ) {}

		public ThreadLocalValue( ThreadLocal<T> local )
		{
			this.local = local;
		}

		public override void Assign( T item ) => local.Value = item;

		public override T Item => local.Value;

		protected override void OnDispose()
		{
			local.Dispose();
			base.OnDispose();
		}
	}

	public abstract class ConnectedValue<T> : WritableValue<T>
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

		[Reference]
		public ConnectibleProperty<T> Property { get; }

		protected override void OnDispose()
		{
			Property.TryDisconnect();
			base.OnDispose();
		}
	}

	public class Reference<T> : ConnectedValue<T>
	{
		public Reference( T key, object instance ) : base( instance, Key( key ), () => key )
		{ }

		public static string Key( object item ) => $"{typeof(T)}-{item.GetHashCode()}";
	}

	class Checked : AssociatedValue<ConditionMonitor>
	{
		public Checked( object instance ) : base( instance, () => new ConditionMonitor() ) { }
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