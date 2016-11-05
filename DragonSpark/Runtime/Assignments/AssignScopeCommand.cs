using DragonSpark.Sources;
using System;

namespace DragonSpark.Runtime.Assignments
{
	public class AssignScopeCommand<T> : AssignCommand<Func<T>>
	{
		public AssignScopeCommand( IAssignable<Func<T>> assignable ) : base( assignable ) {}
	}
}