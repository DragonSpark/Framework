using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Runtime
{
	public class DeclarativeCollection : DeclarativeCollection<object>
	{
		public DeclarativeCollection() {}
		public DeclarativeCollection( IEnumerable<object> collection ) : base( collection ) {}
		public DeclarativeCollection( ICollection<object> items ) : base( items ) {}
	}

	[Ambient]
	public class DeclarativeCollection<T> : CollectionBase<T>, IList<T>
	{
		public DeclarativeCollection() {}
		public DeclarativeCollection( IEnumerable<T> collection ) : base( collection ) {}
		public DeclarativeCollection( ICollection<T> items ) : base( items ) {}

		int IList<T>.IndexOf( T item ) => IndexOf( item );
		void IList<T>.Insert( int index, T item ) => Insert( index, item );
		void IList<T>.RemoveAt( int index ) => RemoveAt( index );
		T IList<T>.this[ int index ]
		{
			get { return (T)this[index]; }
			set { this[index] = value; }
		}
	}
}