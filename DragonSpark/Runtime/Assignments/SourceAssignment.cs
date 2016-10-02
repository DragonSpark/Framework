using DragonSpark.Sources;

namespace DragonSpark.Runtime.Assignments
{
	public class SourceAssignment<T> : IAssign<T>
	{
		readonly IAssignable<T> store;
		public SourceAssignment( IAssignable<T> store )
		{
			this.store = store;
		}

		public void Assign( T first ) => store.Assign( first );
	}
}