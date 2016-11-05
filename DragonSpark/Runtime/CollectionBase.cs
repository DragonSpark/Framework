using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Runtime
{
	public abstract class CollectionBase<T> : IList, ICollection<T>
	{
		readonly Func<T, int> add;
		readonly Func<T, bool> contains;
		readonly Func<object, int> indexOf;
		readonly IList list;

		protected CollectionBase() : this( new List<T>( Items<T>.Default ) ) {}

		protected CollectionBase( IEnumerable<T> items ) : this( new List<T>( items ) ) {}

		protected CollectionBase( ICollection<T> source )
		{
			Source = source;

			add = AddItem;
			contains = Contains;
			list = (IList)Source;
			indexOf = list.IndexOf;
		}

		protected ICollection<T> Source { get; }

		protected virtual IEnumerable<T> Query => Source;

		public IEnumerator<T> GetEnumerator() => Query.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public virtual void Add( T item )
		{
			lock ( Source )
			{
				Source.Add( item );
			}
		}

		int IList.Add( object value )
		{
			var item = OnAdd( value );
			var result =  item.With( add, () => -1 );
			return result;
		}

		protected virtual T OnAdd( object item ) => item.As<T>();

		int AddItem( T arg )
		{
			Add( arg );
			return Count - 1;
		}

		public void Clear() => Source.Clear();

		bool IList.Contains( object value ) => value.AsTo( contains );

		public int IndexOf( object value ) => value.AsTo( indexOf, () => -1 );

		public void Insert( int index, object value ) => value.As<T>( x => list.Insert( index, x ) );

		void IList.Remove( object value ) => value.As<T>( x => Source.Remove( x ) );

		public void RemoveAt( int index ) => list.RemoveAt( index );

		bool IList.IsFixedSize => list.IsFixedSize;

		public bool Contains( T item ) => Source.Contains( item );

		public virtual void CopyTo( T[] array, int arrayIndex ) => Query.ToArray().CopyTo( array, arrayIndex );

		public bool Remove( T item ) => Source.Remove( item );

		void ICollection.CopyTo( Array array, int index ) => array.As<T[]>( obj => CopyTo( obj, index ) );

		public int Count => Source.Count;

		bool ICollection.IsSynchronized => Source.To<ICollection>().IsSynchronized;

		object ICollection.SyncRoot => Source.To<ICollection>().SyncRoot;

		public virtual bool IsReadOnly => Source.To<ICollection<T>>().IsReadOnly;

		public object this[ int index ]
		{
			get { return list[index]; }
			set { list[index] = (T)value; }
		}
	}
}