using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Linq;

namespace DragonSpark.Sources.Parameterized
{
	public class ParameterConstructedCompositeFactory<T> : ParameterConstructedCompositeFactory<object, T>
	{
		public ParameterConstructedCompositeFactory( params Type[] types ) : base( types ) {}
	}

	public class ParameterConstructedCompositeFactory<TParameter, TResult> : CompositeFactory<TParameter, TResult>
	{
		public ParameterConstructedCompositeFactory( params Type[] types ) : base( types.Select( type => new Factory( type ) ).Fixed() ) {}

		sealed class Factory : ParameterizedSourceBase<TParameter, TResult>
		{
			readonly Type type;

			public Factory( Type type )
			{
				this.type = type;
			}

			public override TResult Get( TParameter parameter ) => ParameterConstructor<TResult>.Make( parameter.GetType(), type )( parameter );
		}
	}
}