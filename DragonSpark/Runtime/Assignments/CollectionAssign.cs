using DragonSpark.Commands;
using System;
using System.Collections.Generic;

namespace DragonSpark.Runtime.Assignments
{
	public static class CollectionActions
	{
		public static Value<CollectionAction> Default { get; } = new Value<CollectionAction>( CollectionAction.Add, CollectionAction.Remove );

		public static IDisposable Assignment<T>( this ICollection<T> @this, T item ) => new CollectionAssignment<T>( @this, item ).AsExecuted();
	}

	public sealed class CollectionAssignment<T> : Assignment<T, CollectionAction>
	{
		public CollectionAssignment( ICollection<T> collection, T item ) : base( new CollectionAssign<T>( collection ), Assignments.From( item ), CollectionActions.Default ) {}
		
	}

	public sealed class CollectionAssign<T> : IAssign<T, CollectionAction>
	{
		readonly ICollection<T> collection;

		public CollectionAssign( ICollection<T> collection )
		{
			this.collection = collection;
		}

		public void Assign( T first, CollectionAction second )
		{
			switch ( second )
			{
				case CollectionAction.Add:
					collection.Add( first );
					break;
				case CollectionAction.Remove:
					collection.Remove( first );
					break;
			}
		}
	}
}