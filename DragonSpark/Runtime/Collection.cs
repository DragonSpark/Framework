using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Runtime
{
	/// <summary>
	/// ATTRIBUTION: http://stackoverflow.com/questions/2907372/why-does-c-sharp-not-implement-gethashcode-for-collections
	/// </summary>
	public class EqualityList : List<object>, IEquatable<EqualityList>
	{
		public EqualityList() {}

		public EqualityList( [Required] params object[] items ) : base( items ) {}

		public bool Equals( EqualityList other )
		{
			var result = other.With( list => ReferenceEquals( this, other ) || list.Count == Count && list.GetHashCode() == GetHashCode() );
			return result;
		}

		public override bool Equals( object other ) => other.AsTo<EqualityList, bool>( Equals );

		public override int GetHashCode() => this.Aggregate( 0x2D2816FE, ( current, item ) => current * 31 + ( item?.GetHashCode() ?? 0 ) );
	}

	public class Collection : Collection<object> {}

	[Ambient]
	public class Collection<T> : /*System.Collections.ObjectModel.Collection<T>*/ IList, ICollection<T>
	{
		/*public Collection()
		{
		}

		public Collection( IList<T> list ) // : base( list )
		{
		}*/

		readonly List<T> items;

		// readonly ConditionMonitor built = new ConditionMonitor();

		[InjectionConstructor]
		public Collection() : this( new T[0] )
		{}

		public Collection( IEnumerable<T> collection )
		{
			items = new List<T>( collection );
		}

		// IEnumerable<T> Items => built.Apply() ? items.Select( arg => arg.BuildUp() ) : items;

		public virtual IEnumerator<T> GetEnumerator() => items.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add( T item ) => items.Add( item );

		int IList.Add( object value )
		{
			var item = OnAdd( value );
			var result =  item.With( AddItem, () => -1 );
			return result;
		}

		protected virtual T OnAdd( object item ) => item.As<T>();

		int AddItem( T arg )
		{
			Add( arg );
			return Count - 1;
		}

		public void Clear() => items.Clear();

		bool IList.Contains( object value ) => value.AsTo<T, bool>( Contains );

		int IList.IndexOf( object value ) => value.AsTo<T, int>( items.IndexOf, () => -1 );

		void IList.Insert( int index, object value ) => value.As<T>( x => items.Insert( index, x ) );

		void IList.Remove( object value ) => value.As<T>( x => items.Remove( x ) );

		void IList.RemoveAt( int index ) => items.RemoveAt( index );

		bool IList.IsFixedSize => items.To<IList>().IsFixedSize;

		public bool Contains( T item ) => items.Contains( item );

		public void CopyTo( T[] array, int arrayIndex ) => items.ToArray().CopyTo( array, arrayIndex );

		public bool Remove( T item ) => items.Remove( item );

		void ICollection.CopyTo( Array array, int index ) => array.As<T[]>( obj => CopyTo( obj, index ) );

		public int Count => items.Count;

		bool ICollection.IsSynchronized => items.To<ICollection>().IsSynchronized;

		object ICollection.SyncRoot => items.To<ICollection>().SyncRoot;

		public virtual bool IsReadOnly => items.To<ICollection<T>>().IsReadOnly;

		object IList.this[ int index ]
		{
			get { return items[index]; }
			set { items[index] = (T)value; }
		}
	}
}