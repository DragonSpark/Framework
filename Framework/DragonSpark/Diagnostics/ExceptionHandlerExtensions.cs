using System;
using DragonSpark.Extensions;

namespace DragonSpark.Diagnostics
{
	public static class ExceptionHandlerExtensions
	{
		public static void Process( this IExceptionHandler target, Exception exception )
		{
			target.Handle( exception ).With( a => a.RethrowRecommended.IsTrue( () => { throw a.Exception; } ) );
		}
	}
}