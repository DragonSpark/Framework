using DragonSpark.Commands;
using DragonSpark.Sources;

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
}