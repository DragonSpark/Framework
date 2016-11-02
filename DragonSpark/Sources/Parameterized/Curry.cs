using System;

namespace DragonSpark.Sources.Parameterized
{
	public class Curry<T1, TResult> : Curry<T1, object, TResult>
	{
		public Curry( Func<T1, Func<object, TResult>> factory ) : base( factory ) {}
	}
}