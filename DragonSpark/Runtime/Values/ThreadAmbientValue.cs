using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Runtime.Values
{
	public class ThreadAmbientContext : ThreadValueBase<object>
	{
		public ThreadAmbientContext() : base( typeof(ThreadAmbientContext), () => new object() ) { }

		public static object GetCurrent() => new ThreadAmbientContext().Item;
	}

	public abstract class ThreadValueBase<T> : DecoratedValue<T>
	{
		readonly ConnectedValue<ThreadLocalValue<T>> inner;

		protected ThreadValueBase( object instance, Func<T> create = null ) : this( new AssociatedValue<ThreadLocalValue<T>>( instance, () => new ThreadLocalValue<T>( create ) ) ) {}

		protected ThreadValueBase( [Required]ConnectedValue<ThreadLocalValue<T>> inner ) : base( inner.Item )
		{
			this.inner = inner;
		}

		protected override void OnDispose() => inner.Dispose();
	}

	public class ThreadAmbientValue<T> : ThreadValueBase<Stack<T>>
	{
		public ThreadAmbientValue() : base( ThreadAmbientContext.GetCurrent(), () => new Stack<T>() ) {}

		protected override void OnDispose()
		{
			Item.Clear();
			base.OnDispose();
		}
	}
}