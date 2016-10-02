using System.Collections;

namespace DragonSpark.Runtime.Assignments
{
	public class CollectionAssign<T> : IAssign<T, CollectionAction>
	{
		readonly IList collection;

		public CollectionAssign( IList collection )
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