using System;
using DragonSpark.Sources;

namespace DragonSpark.Runtime.Assignments
{
	public class AssignScopeCommand<T> : AssignCommand<Func<T>>
	{
		public AssignScopeCommand( IAssignable<Func<T>> assignable ) : base( assignable ) {}
	}
}