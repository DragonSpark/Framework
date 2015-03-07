using System;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;

namespace DragonSpark.Application.IoC.Commands
{
	public static class ExceptionHandlerExtensions
	{
		public static void Process( this IExceptionHandler target, Exception exception )
		{
			target.Handle( exception ).With( a => a.RethrowRecommended.IsTrue( () => { throw a.Exception; } ) );
		}
	}
}