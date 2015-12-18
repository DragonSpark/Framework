using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
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

		void Dispose( bool disposing )
		{
			disposing.IsTrue( OnDispose );
		}

		protected virtual void OnDispose()
		{
			AmbientValues.Remove( GetType() );
		}
	}
}