using System;

namespace DragonSpark.Runtime.Values
{
	public class AmbientContext : IDisposable
	{
		readonly IAmbientKey key;

		public AmbientContext( IAmbientKey key, object item )
		{
			this.key = key;
			AmbientValues.Register( key, item );
		}

		public void Dispose()
		{
			AmbientValues.Remove( key );
		}
	}
}