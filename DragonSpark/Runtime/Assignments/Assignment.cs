using System;

namespace DragonSpark.Runtime.Assignments
{
	public sealed class Assignment<T> : DisposableBase
	{
		readonly IAssign<T> assign;
		readonly Value<T> first;
		
		public Assignment( IAssign<T> assign, Value<T> first )
		{
			this.assign = assign;
			this.first = first;
			
			assign.Assign( first.Start );
		}

		protected override void OnDispose( bool disposing ) => assign.Assign( first.Finish );
	}

	public class Assignment<T1, T2> : DisposableBase
	{
		readonly IAssign<T1, T2> assign;
		readonly Value<T1> first;
		readonly Value<T2> second;

		public Assignment( IAssign<T1, T2> assign, T1 first, T2 second ) : this( assign, Assignments.From( first ), new Value<T2>( second ) ) {}

		public Assignment( IAssign<T1, T2> assign, Value<T1> first, Value<T2> second )
		{
			this.assign = assign;
			this.first = first;
			this.second = second;

			assign.Assign( first.Start, second.Start );
		}

		protected override void OnDispose( bool disposing ) => assign.Assign( first.Finish, second.Finish );
	}

	public struct CacheAssignment<T1, T2> : IDisposable
	{
		readonly CacheAssign<T1, T2> assign;
		readonly Value<T1> first;
		readonly Value<T2> second;

		public CacheAssignment( CacheAssign<T1, T2> assign, T1 instance, T2 start, T2 finish = default(T2) ) : this( assign, Assignments.From( instance ), new Value<T2>( start, finish ) ) {}

		public CacheAssignment( CacheAssign<T1, T2> assign, Value<T1> first, Value<T2> second )
		{
			this.assign = assign;
			this.first = first;
			this.second = second;

			assign.Assign( first.Start, second.Start );
		}

		public void Dispose() => assign.Assign( first.Finish, second.Finish );
	}
}