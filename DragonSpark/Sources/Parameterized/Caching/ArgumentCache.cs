using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class ArgumentCache<TArgument, TValue> : CacheBase<TArgument, TValue>, IArgumentCache<TArgument, TValue>
	{
		static IEqualityComparer<TArgument> EqualityComparer { get; } = typeof(TArgument).IsStructural() ? (IEqualityComparer<TArgument>)StructuralEqualityComparer<TArgument>.Default : EqualityComparer<TArgument>.Default;

		readonly Func<TArgument, TValue> body;
		readonly ConcurrentDictionary<TArgument, TValue> store = new ConcurrentDictionary<TArgument, TValue>( EqualityComparer );

		public ArgumentCache() : this( argument => default(TValue) ) {}

		public ArgumentCache( Func<TArgument, TValue> body )
		{
			this.body = body;
		}

		public override bool Contains( TArgument instance ) => store.ContainsKey( instance );

		public override bool Remove( TArgument instance )
		{
			TValue removed;
			return store.TryRemove( instance, out removed );
		}

		public override void Set( TArgument instance, TValue value ) => store[instance] = value;

		public override TValue Get( TArgument parameter ) => store.GetOrAdd( parameter, body );

		public virtual TValue GetOrSet( TArgument key, Func<TValue> factory )
		{
			TValue result;
			return store.TryGetValue( key, out result ) ? result : store.GetOrAdd( key, factory() );
		}

		public TValue GetOrSet( TArgument key, Func<TArgument, TValue> factory ) => store.GetOrAdd( key, factory );
	}
}