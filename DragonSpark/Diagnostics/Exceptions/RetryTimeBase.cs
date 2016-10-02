using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.Diagnostics.Exceptions
{
	public abstract class RetryTimeBase : ParameterizedSourceBase<int, TimeSpan>
	{
		readonly Alter<int> time;

		protected RetryTimeBase( Alter<int> time )
		{
			this.time = time;
		}

		public override TimeSpan Get( int parameter ) => TimeSpan.FromSeconds( time( parameter ) );
	}
}