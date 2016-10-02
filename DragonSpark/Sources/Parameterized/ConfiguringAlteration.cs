using System;

namespace DragonSpark.Sources.Parameterized
{
	public class ConfiguringAlteration<T> : AlterationBase<T>
	{
		readonly Action<T> configure;

		public ConfiguringAlteration( Action<T> configure )
		{
			this.configure = configure;
		}

		public override T Get( T parameter )
		{
			configure( parameter );
			return parameter;
		}
	}
}