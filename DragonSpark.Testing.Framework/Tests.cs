using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Framework
{
	public abstract class Tests : IDisposable
	{
		protected Tests( ITestOutputHelper output )
		{
			AmbientValues.RegisterFor( output, GetType() );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			disposing.IsTrue( () => AmbientValues.Remove( GetType() ) );
		}
	}
}