using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using System.Linq;

namespace DragonSpark.Sources.Parameterized
{
	public class ParameterConstructedCompositeFactory<T> : CompositeFactory<object, T>
	{
		public ParameterConstructedCompositeFactory( params Type[] types ) : base( types.Select( type => new Factory( type ) ).Fixed() ) {}

		sealed class Factory : ParameterizedSourceBase<T>
		{
			readonly Type type;

			public Factory( Type type )
			{
				this.type = type;
			}

			public override T Get( object parameter ) => ParameterConstructor<T>.Make( parameter.GetType(), type )( parameter );
		}
	}
}