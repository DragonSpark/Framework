using System;
using System.Diagnostics;

namespace DragonSpark.Logging
{
	public sealed class Indentation : IDisposable
	{
		public static TResult Process<TResult>( Func<TResult> target )
		{
			using ( new Indentation() )
			{
				return target();
			}
		}

		Indentation()
		{
			Trace.Indent();
		}

		public void Dispose()
		{
			Trace.Unindent();
		}
	}
}