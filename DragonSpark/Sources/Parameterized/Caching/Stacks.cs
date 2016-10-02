using DragonSpark.Runtime;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class Stacks<T> : DecoratedSourceCache<IStack<T>>
	{
		public static Stacks<T> Default { get; } = new Stacks<T>();

		public Stacks() : this( CacheRegistry<IStack<T>>.Default ) {}

		protected Stacks( ICacheRegistry<IStack<T>> registry ) : this( registry, new Store( registry.Clear ) ) {}

		protected Stacks( ICacheRegistry<IStack<T>> registry, IParameterizedSource<IAssignableSource<IStack<T>>> factory ) : base( new ThreadLocalSourceCache<IStack<T>>( factory.Get ) )
		{
			registry.Register( factory, this );
		}

		sealed class Store : ParameterizedSourceBase<IAssignableSource<IStack<T>>>
		{
			readonly Action<Store, object> callback;

			public Store( Action<Store, object> callback )
			{
				this.callback = callback;
			}

			public override IAssignableSource<IStack<T>> Get( object instance ) => new Factory( this, instance ).Get();

			sealed class Factory : DisposableBase
			{
				readonly Store owner;
				readonly object instance;
				readonly ThreadLocalStore<IStack<T>> store;
				readonly ConcurrentDictionary<IStack<T>, bool> empty = new ConcurrentDictionary<IStack<T>, bool>();
				readonly ThreadLocal<IStack<T>> local;
				readonly Action<IStack<T>> onEmpty;
				readonly Func<IStack<T>, bool> isEmpty;

				public Factory( Store owner, object instance )
				{
					this.owner = owner;
					this.instance = instance;

					local = new ThreadLocal<IStack<T>>( New, true );
					store = new ThreadLocalStore<IStack<T>>( local );
					onEmpty = OnEmpty;
					isEmpty = IsEmpty;
				}

				public IAssignableSource<IStack<T>> Get() => store;

				IStack<T> New() => new Stack<T>( onEmpty );

				void OnEmpty( IStack<T> item )
				{
					if ( empty.TryAdd( item, true ) && local.Values.All( isEmpty ) )
					{
						empty.Clear();
						owner.Clear( instance );
						store.Dispose();
					}
				}

				bool IsEmpty( IStack<T> stack )
				{
					bool stored;
					return empty.ContainsKey( stack ) && empty.TryGetValue( stack, out stored ) && stored;
				}

				protected override void OnDispose( bool disposing ) => store.Dispose();
			}

			void Clear( object instance ) => callback( this, instance );
		}
	}
}