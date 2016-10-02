using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class Stack<T> : IStack<T>
	{
		readonly System.Collections.Generic.Stack<T> store;
		readonly Action<IStack<T>> onEmpty;
		public Stack() : this( Delegates<IStack<T>>.Empty ) {}

		public Stack( System.Collections.Generic.Stack<T> store ) : this( store, Delegates<IStack<T>>.Empty ) {}

		public Stack( Action<IStack<T>> onEmpty ) : this( new System.Collections.Generic.Stack<T>(), onEmpty ) {}

		public Stack( System.Collections.Generic.Stack<T> store, Action<IStack<T>> onEmpty )
		{
			this.store = store;
			this.onEmpty = onEmpty;
		}

		public bool Contains( T item ) => store.Contains( item );

		public ImmutableArray<T> All() => store.ToImmutableArray();

		public T Peek() => store.PeekOrDefault();

		public void Push( T item ) => store.Push( item );

		public T Pop()
		{
			var result = store.Pop();
			if ( !store.Any() )
			{
				onEmpty( this );
			}
			return result;
		}
	}
}