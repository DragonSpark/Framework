using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Runtime.Assignments
{
	public class AssignWithRestoreCommand<T> : AssignWithDisposeCommand<T>
	{
		public AssignWithRestoreCommand( IAssignable<T> assignable, T current ) : base( assignable, new Assignment<T>( new Assign<T>( assignable ), current ) ) {}
	}

	public class AssignWithDisposeCommand<T> : DisposingCommandBase<T>
	{
		readonly IAssignable<T> assignable;

		public AssignWithDisposeCommand( IAssignable<T> assignable ) : this( assignable, assignable.AsDisposable() ) {}

		protected AssignWithDisposeCommand( IAssignable<T> assignable, IDisposable disposable ) : base( disposable )
		{
			this.assignable = assignable;
		}

		public override void Execute( T parameter ) => assignable.Assign( parameter );
	}
}