using DragonSpark.Commands;

namespace DragonSpark.Runtime.Assignments
{
	public class Assignment<T> : DisposableBase
	{
		readonly IAssign<T> assign;
		readonly T finish;

		public Assignment( IAssign<T> assign, T finish )
		{
			this.assign = assign;
			this.finish = finish;
		}

		protected override void OnDispose( bool disposing ) => assign.Assign( finish );
	}

	public sealed class ValueAssignment<T> : Assignment<T>, IExecution
	{
		readonly IAssign<T> assign;
		readonly T start;

		public ValueAssignment( IAssign<T> assign, Value<T> value ) : this( assign, value.Start, value.Finish ) {}

		ValueAssignment( IAssign<T> assign, T start, T finish ) : base( assign, finish )
		{
			this.assign = assign;
			this.start = start;
		}

		public void Execute() => assign.Assign( start );
	}

	public class Assignment<T1, T2> : DisposableBase, IExecution
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
		}

		public void Execute() => assign.Assign( first.Start, second.Start );

		protected override void OnDispose( bool disposing ) => assign.Assign( first.Finish, second.Finish );
	}
}