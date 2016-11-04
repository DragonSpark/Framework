using System;
using DragonSpark.Sources;

namespace DragonSpark.Runtime.Assignments
{
	public class AssignGlobalScopeCommand<T> : AssignCommand<Func<object, T>>
	{
		public AssignGlobalScopeCommand( IAssignable<Func<object, T>> assignable ) : base( assignable ) {}
	}
}