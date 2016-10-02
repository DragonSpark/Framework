using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DragonSpark.Runtime
{
	// ATTRIBUTION: https://github.com/dotnet/roslyn/blob/master/src/Compilers/Core/Portable/InternalUtilities/WeakList.cs
	public sealed class WeakList<T> : ICollection<T> where T : class
	{
		const int MinimalNonEmptySize = 4;

		WeakReference<T>[] items = Items<WeakReference<T>>.Default;
		int size;

		void Resize()
		{
			Debug.Assert( size == items.Length );
			Debug.Assert( items.Length == 0 || items.Length >= MinimalNonEmptySize );

			var alive = items.Length;
			var firstDead = -1;
			for ( int i = 0; i < items.Length; i++ )
			{
				T target;
				if ( !items[i].TryGetTarget( out target ) )
				{
					if ( firstDead == -1 )
					{
						firstDead = i;
					}

					alive--;
				}
			}

			if ( alive < items.Length / 4 )
			{
				// If we have just a few items left we shrink the array.
				// We avoid expanding the array until the number of new items added exceeds half of its capacity.
				Shrink( firstDead, alive );
			}
			else if ( alive >= 3 * items.Length / 4 )
			{
				// If we have a lot of items alive we expand the array since just compacting them 
				// wouldn't free up much space (we would end up calling Resize again after adding a few more items).
				var newItems = new WeakReference<T>[GetExpandedSize( items.Length )];

				if ( firstDead >= 0 )
				{
					Compact( firstDead, newItems );
				}
				else
				{
					Array.Copy( items, 0, newItems, 0, items.Length );
					Debug.Assert( size == items.Length );
				}

				items = newItems;
			}
			else
			{
				// Compact in-place to make space for new items at the end.
				// We will free up to length/4 slots in the array.
				Compact( firstDead, items );
			}

			Debug.Assert( items.Length > 0 && size < 3 * items.Length / 4, "length: " + items.Length + " size: " + size );
		}

		void Shrink( int firstDead, int alive )
		{
			var newSize = GetExpandedSize( alive );
			var newItems = newSize == items.Length ? items : new WeakReference<T>[newSize];
			Compact( firstDead, newItems );
			items = newItems;
		}

		static int GetExpandedSize( int baseSize ) => Math.Max( baseSize * 2 + 1, MinimalNonEmptySize );

		/// <summary>
		/// Copies all live references from <see cref="items"/> to <paramref name="result"/>.
		/// Assumes that all references prior <paramref name="firstDead"/> are alive.
		/// </summary>
		void Compact( int firstDead, WeakReference<T>[] result )
		{
			// Debug.Assert(_items[firstDead].IsNull());

			if ( !ReferenceEquals( items, result ) )
			{
				Array.Copy( items, 0, result, 0, firstDead );
			}

			var oldSize = size;
			var j = firstDead;
			for ( var i = firstDead + 1; i < oldSize; i++ )
			{
				var item = items[i];

				T target;
				if ( item.TryGetTarget( out target ) )
				{
					result[j++] = item;
				}
			}

			size = j;

			// free WeakReferences
			if ( ReferenceEquals( items, result ) )
			{
				while ( j < oldSize )
				{
					items[j++] = null;
				}
			}
		}

		public int WeakCount => size;

		public WeakReference<T> GetWeakReference( int index )
		{
			if ( index < 0 || index >= size )
			{
				throw new ArgumentOutOfRangeException( nameof( index ) );
			}

			return items[index];
		}

		public void Add( T item )
		{
			if ( size == items.Length )
			{
				Resize();
			}

			Debug.Assert( size < items.Length );
			items[size++] = new WeakReference<T>( item );
		}

		public void Clear() => items.WhereAssigned().ForEach( reference => reference.SetTarget( null ) );
		public bool Contains( T item ) => Items().Contains( item );

		public void CopyTo( T[] array, int arrayIndex ) => Array.Copy( Items().ToArray(), arrayIndex, array, arrayIndex, array.Length );

		public bool Remove( T item )
		{
			var index = Array.IndexOf( Items().ToArray(), item );
			var result = index > 1;
			if ( result )
			{
				items[index].SetTarget( null );
			}
			return result;
		}

		IEnumerable<T> Items() => items.Select( reference => reference?.Get() );

		public int Count => WeakCount;

		public bool IsReadOnly => false;

		public IEnumerator<T> GetEnumerator()
		{
			int count = size;
			int alive = size;
			int firstDead = -1;

			for ( int i = 0; i < count; i++ )
			{
				T item;
				if ( items[i].TryGetTarget( out item ) )
				{
					yield return item;
				}
				else
				{
					// object has been collected 

					if ( firstDead < 0 )
					{
						firstDead = i;
					}

					alive--;
				}
			}

			if ( alive == 0 )
			{
				items = Items<WeakReference<T>>.Default;
				size = 0;
			}
			else if ( alive < items.Length / 4 )
			{
				// If we have just a few items left we shrink the array.
				Shrink( firstDead, alive );
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}