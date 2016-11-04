using DragonSpark.Commands;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Runtime.Assignments
{
	public class AssignCommand<T> : CommandBase<T>
	{
		readonly IAssignable<T> assignable;

		public AssignCommand( IAssignable<T> assignable )
		{
			this.assignable = assignable;
		}

		public override void Execute( T parameter ) => assignable.Assign( parameter );
	}

	public class AssignScopeCommand<T> : AssignCommand<Func<T>>
	{
		public AssignScopeCommand( IAssignable<Func<T>> assignable ) : base( assignable ) {}
	}

	public class AssignGlobalScopeCommand<T> : AssignCommand<Func<object, T>>
	{
		public AssignGlobalScopeCommand( IAssignable<Func<object, T>> assignable ) : base( assignable ) {}
	}
}