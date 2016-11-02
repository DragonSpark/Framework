using System;

namespace DragonSpark.Sources.Parameterized
{
	public class AlterationAdapter<T> : AlterationBase<T>
	{
		readonly Func<T, T> factory;

		public AlterationAdapter( Func<T, T> factory )
		{
			this.factory = factory;
		}

		public override T Get( T parameter ) => factory( parameter );
	}
}