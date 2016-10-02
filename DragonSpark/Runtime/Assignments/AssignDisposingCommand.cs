using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources;

namespace DragonSpark.Runtime.Assignments
{
	public class AssignWithRestoreCommand<T> : AssignDisposingCommand<T>
	{
		readonly T current;
		public AssignWithRestoreCommand( IAssignable<T> assignable, T current ) : base( assignable )
		{
			this.current = current;
		}

		protected override void OnDispose()
		{
			Assignable.Assign( current );
			base.OnDispose();
		}
	}

	public class AssignDisposingCommand<T> : DisposingCommand<T>
	{
		public AssignDisposingCommand( IAssignable<T> assignable )
		{
			Assignable = assignable;
		}

		protected IAssignable<T> Assignable { get; }

		public override void Execute( T parameter ) => Assignable.Assign( parameter );

		protected override void OnDispose()
		{
			Assignable.TryDispose();
			base.OnDispose();
		}
	}

	public class AssignCommand<T> : CommandBase<T>
	{
		readonly IAssignable<T> assignable;

		public AssignCommand( IAssignable<T> assignable )
		{
			this.assignable = assignable;
		}

		public override void Execute( T parameter ) => assignable.Assign( parameter );
	}
}