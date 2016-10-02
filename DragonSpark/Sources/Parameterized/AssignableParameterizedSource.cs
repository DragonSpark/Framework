using System;

namespace DragonSpark.Sources.Parameterized
{
	public class AssignableParameterizedSource<TParameter, TResult> : SuppliedSource<Func<TParameter, TResult>>, IAssignableParameterizedSource<TParameter, TResult>
	{
		public AssignableParameterizedSource( Func<TParameter, TResult> reference ) : base( reference ) {}
		public TResult Get( TParameter parameter ) => Get()( parameter);
	}
}