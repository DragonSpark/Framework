using DragonSpark.Extensions;
using System;

namespace DragonSpark.Runtime
{
	public interface IExceptionHandler
	{
		ExceptionHandlingResult Handle( Exception exception );
	}

	public static class ExceptionHandlerExtensions
	{
		public static void Process( this IExceptionHandler target, Exception exception )
		{
			target.Handle( exception ).With( a => a.RethrowRecommended.IsTrue( () => { throw a.Exception; } ) );
		}
	}
}