using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.Runtime
{
	// FROM: http://nitokitchensink.codeplex.com/SourceControl/changeset/view/51391#1006413

	/// <summary>
	///     A collection of weak references to objects of type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T">The type of objects to hold weak references to.</typeparam>
	public interface IWeakCollection<T> : ICollection<T>, IDisposable where T : class
	{
		/// <summary>
		///     Gets a sequence of live objects from the collection, causing a purge.
		/// </summary>
		IEnumerable<T> Alive { get; }

		/// <summary>
		///     Gets a complete sequence of objects from the collection. Does not cause a purge. Null entries represent dead
		///     objects.
		/// </summary>
		IEnumerable<T> All { get; }

		/// <summary>
		///     Gets a sequence of live objects from the collection without causing a purge.
		/// </summary>
		IEnumerable<T> LiveListWithoutPurge { get; }

		/// <summary>
		///     Gets the number of live and dead entries in the collection. Does not cause a purge. O(1).
		/// </summary>
		int CompleteCount { get; }

		/// <summary>
		///     Gets the number of dead entries in the collection. Does not cause a purge. O(n).
		/// </summary>
		int DeadCount { get; }

		/// <summary>
		///     Gets the number of live entries in the collection, causing a purge. O(n).
		/// </summary>
		int LiveCount { get; }

		/// <summary>
		///     Gets the number of live entries in the collection without causing a purge. O(n).
		/// </summary>
		int LiveCountWithoutPurge { get; }

		/// <summary>
		///     Removes all dead objects from the collection.
		/// </summary>
		void Purge();
	}

	/// <summary>
	///     A collection of weak references to objects. Weak references are purged by iteration/count operations, not by
	///     add/remove operations.
	/// </summary>
	/// <typeparam name="T">The type of object to hold weak references to.</typeparam>
	/// <remarks>
	///     <para>
	///         Since the collection holds weak references to the actual objects, the collection is comprised of both living
	///         and dead references. Living references refer to objects that have not been garbage collected, and may be used
	///         as normal references. Dead references refer to objects that have been garbage collected.
	///     </para>
	///     <para>Dead references do consume resources; each dead reference is a garbage collection handle.</para>
	///     <para>
	///         Dead references may be cleaned up by a <see cref="Purge" /> operation. Some properties and methods cause a
	///         purge as a side effect; the member documentation specifies whether a purge takes place.
	///     </para>
	/// </remarks>
	public sealed class WeakCollection<T> : IWeakCollection<T> where T : class
	{
		/// <summary>
		///     The actual collection of strongly-typed weak references.
		/// </summary>
		readonly List<WeakReference<T>> list = new List<WeakReference<T>>();

		/// <summary>
		///     Gets a sequence of live objects from the collection, causing a purge.
		/// </summary>
		public IEnumerable<T> Alive => new ReadOnlyCollection<T>( UnsafeLiveList.ToList() );

		/// <summary>
		///     Gets a complete sequence of objects from the collection. Does not cause a purge. Null entries represent dead
		///     objects.
		/// </summary>
		public IEnumerable<T> All
		{
			get
			{
				return list.Select( x =>
				{
					T item;
					return x.TryGetTarget( out item ) ? item : default(T);
				} );
			}
		}

		/// <summary>
		///     Gets a sequence of live objects from the collection without causing a purge.
		/// </summary>
		public IEnumerable<T> LiveListWithoutPurge
		{
			get { return All.Where( x => x != default(T) ); }
		}

		/// <summary>
		///     Gets the number of live and dead entries in the collection. Does not cause a purge. O(1).
		/// </summary>
		public int CompleteCount => list.Count;

		/// <summary>
		///     Gets the number of dead entries in the collection. Does not cause a purge. O(n).
		/// </summary>
		public int DeadCount
		{
			get { return All.Count( x => x == default(T) ); }
		}

		/// <summary>
		///     Gets the number of live entries in the collection, causing a purge. O(n).
		/// </summary>
		public int LiveCount => UnsafeLiveList.Count();

		/// <summary>
		///     Gets the number of live entries in the collection without causing a purge. O(n).
		/// </summary>
		public int LiveCountWithoutPurge
		{
			get { return All.Count( x => x != null ); }
		}

		#region ICollection<T> Properties
		/// <summary>
		///     Gets the number of live entries in the collection, causing a purge. O(n).
		/// </summary>
		int ICollection<T>.Count => LiveCount;

		/// <summary>
		///     Gets a value indicating whether the collection is read only. Always returns false.
		/// </summary>
		bool ICollection<T>.IsReadOnly => false;
		#endregion

		/// <summary>
		///     Gets a sequence of live objects from the collection, causing a purge. The entire sequence MUST always be
		///     enumerated!
		/// </summary>
		private IEnumerable<T> UnsafeLiveList
		{
			get
			{
				// This implementation uses logic similar to List<T>.RemoveAll, which always has O(n) time.
				//  Some other implementations seen in the wild have O(n*m) time, where m is the number of dead entries.
				//  As m approaches n (e.g., mass object extinctions), their running time approaches O(n^2).
				var writeIndex = 0;
				for ( var readIndex = 0; readIndex != list.Count; ++readIndex )
				{
					var weakReference = list[readIndex];

					T weakDelegate;
					if ( weakReference.TryGetTarget( out weakDelegate ) )
					{
						yield return weakDelegate;

						if ( readIndex != writeIndex )
						{
							list[writeIndex] = list[readIndex];
						}

						++writeIndex;
					}
					/*else
					{
						weakReference.Dispose();
					}*/
				}

				list.RemoveRange( writeIndex, list.Count - writeIndex );
			}
		}

		/// <summary>
		///     Adds a weak reference to an object to the collection. Does not cause a purge.
		/// </summary>
		/// <param name="item">The object to add a weak reference to.</param>
		public void Add( T item )
		{
			list.Add( new WeakReference<T>( item ) );
		}

		/// <summary>
		///     Removes a weak reference to an object from the collection. Does not cause a purge.
		/// </summary>
		/// <param name="item">The object to remove a weak reference to.</param>
		/// <returns>True if the object was found and removed; false if the object was not found.</returns>
		public bool Remove( T item )
		{
			for ( var i = 0; i != list.Count; ++i )
			{
				var weakReference = list[i];
				T weakDelegate;
				if ( weakReference.TryGetTarget( out weakDelegate ) && weakDelegate == item )
				{
					list.RemoveAt( i );
					return true;
				}
			}

			return false;
		}

		/// <summary>
		///     Removes all dead objects from the collection.
		/// </summary>
		public void Purge()
		{
			// We cannot simply use List<T>.RemoveAll, because the predicate "x => !x.IsAlive" is not stable.
			var enumerator = UnsafeLiveList.GetEnumerator();
			while ( enumerator.MoveNext() )
			{}
		}

		/// <summary>
		///     Frees all resources held by the collection.
		/// </summary>
		public void Dispose()
		{
			Clear();
		}

		/// <summary>
		///     Empties the collection.
		/// </summary>
		public void Clear()
		{
			list.Clear();
		}

		#region ICollection<T> Methods
		/// <summary>
		///     Determines whether the collection contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate.</param>
		/// <returns>True if the collection contains a specific value; false if it does not.</returns>
		bool ICollection<T>.Contains( T item )
		{
			return LiveListWithoutPurge.Contains( item );
		}

		/// <summary>
		///     Copies all live objects to an array.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="arrayIndex">The index to begin writing into the array.</param>
		void ICollection<T>.CopyTo( T[] array, int arrayIndex )
		{
			var ret = new List<T>( list.Count );
			ret.AddRange( UnsafeLiveList );
			ret.CopyTo( array, arrayIndex );
		}
		#endregion

		#region IEnumerable<T> Members
		/// <summary>
		///     Gets a sequence of live objects from the collection, causing a purge.
		/// </summary>
		/// <returns>The sequence of live objects.</returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Alive.GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		///     Gets a sequence of live objects from the collection, causing a purge.
		/// </summary>
		/// <returns>The sequence of live objects.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ( (IEnumerable<T>)this ).GetEnumerator();
		}
		#endregion
	}
}