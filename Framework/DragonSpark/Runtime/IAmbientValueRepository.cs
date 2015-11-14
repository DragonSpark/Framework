using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Runtime
{
	public interface IAmbientValueRepository
	{
		void Add( object key, object instance );

		object Get( object key );

		void Remove( object key );
	}

	public static class AmbientValues
	{
		public static void Initialize( IAmbientValueRepository repository )
		{
			Repository = repository;
		}	static IAmbientValueRepository Repository { get; set; } = AmbientValueRepository.Instance;
		
		public static T Get<T>( object context = null )
		{
			var result = (T)Repository.Get( context );
			return result;
		}

		public static void Register( object context, object instance )
		{
			Repository.Add( context, instance );
		}

		public static void Clear( object context )
		{
			Repository.Remove( context );
		}
	}

	class AmbientValueRepository : AmbientValueRepository<object, object>
	{
		public static IAmbientValueRepository Instance { get; } = new AmbientValueRepository();
	}

	public class CompositeAmbientValueRepository : IAmbientValueRepository
	{
		readonly IAmbientValueRepository[] repositories;

		public CompositeAmbientValueRepository( params IAmbientValueRepository[] repositories )
		{
			this.repositories = repositories;
		}

		public void Add( object key, object instance )
		{
			repositories.Apply( repository => repository.Add( key, instance ) );
		}

		public object Get( object key )
		{
			var result = repositories.Select( repository => repository.Get( key ) ).FirstOrDefault( x => x != null );
			return result;
		}

		public void Remove( object key )
		{
			repositories.Apply( repository => repository.Remove( key ) );
		}
	}

	public class AmbientValueRepository<TKey, TValue> : IAmbientValueRepository
	{
		protected IDictionary<TKey, TValue> Items { get; } = new Dictionary<TKey, TValue>();

		public object Get( object key )
		{
			var result = key == null || key is TKey ? GetValue( (TKey)key ) : default(TValue);
			return result;
		}

		protected virtual TValue GetValue( TKey key )
		{
			var result = Items.TryGet( key );
			return result;
		}

		void IAmbientValueRepository.Add( object key, object instance )
		{
			if ( key is TKey && instance is TValue )
			{
				var k = (TKey)key;
				if ( !Items.ContainsKey( k ) )
				{
					OnAdd( k, (TValue)instance );
				}
				else
				{
					throw new InvalidOperationException( $"Key already exists for ambient value: {key} - {Items[k]}" );
				}
			}
		}

		protected virtual void OnAdd( TKey key, TValue instance )
		{
			Items.Add( key, instance );
		}

		void IAmbientValueRepository.Remove( object key )
		{
			if ( key is TKey )
			{
				var k = (TKey)key;
				if ( Items.ContainsKey( k ) )
				{
					OnRemove( k );
				}
				else
				{
					throw new InvalidOperationException( $"Ambient value with key does not exist and therefore cannot be removed: {k}" );
				}
			}
		}

		protected virtual void OnRemove( TKey key )
		{
			Items.Remove( key );
		}
	}

	/*public interface IAmbientValue<out T>
	{
		T Current { get; }
	}*/
}