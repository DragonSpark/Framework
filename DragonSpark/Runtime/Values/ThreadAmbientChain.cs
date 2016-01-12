using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace DragonSpark.Runtime.Values
{
	public class ThreadAmbientContext : ThreadValueBase<object>
	{
		public ThreadAmbientContext() : base( typeof(ThreadAmbientContext), () => new object() ) { }

		public static object GetCurrent() => new ThreadAmbientContext().Item;
	}

	public abstract class DecoratedAssociatedValue<T> : DecoratedValue<T>
	{
		readonly ConnectedValue<IWritableValue<T>> inner;

		protected DecoratedAssociatedValue( object instance, Func<IWritableValue<T>> create = null ) : this( new AssociatedValue<IWritableValue<T>>( instance, create ) ) { }

		protected DecoratedAssociatedValue( [Required]ConnectedValue<IWritableValue<T>> inner ) : base( inner.Item )
		{
			this.inner = inner;
		}

		protected override void OnDispose()
		{
			inner.TryDispose();
			base.OnDispose();
		}
	}

	public abstract class ThreadValueBase<T> : DecoratedAssociatedValue<T>
	{
		protected ThreadValueBase( object instance, Func<T> create = null ) : base( instance, () => new ThreadLocalValue<T>( create ) ) {}
	}

	public class ThreadAmbientChain<T> : ThreadValueBase<Stack<T>>
	{
		public ThreadAmbientChain() : base( ThreadAmbientContext.GetCurrent(), () => new Stack<T>() ) {}

		protected override void OnDispose()
		{
			Item.Clear();
			base.OnDispose();
		}
	}
}