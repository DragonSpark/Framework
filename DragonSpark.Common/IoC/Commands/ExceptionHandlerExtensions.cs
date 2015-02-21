using System;
using DragonSpark.Extensions;

namespace DragonSpark.Common.IoC.Commands
{
	public static class ExceptionHandlerExtensions
	{
		public static void Process( this Diagnostics.IExceptionHandler target, Exception exception )
		{
			target.Handle( exception ).With( a => a.RethrowRecommended.IsTrue( () => { throw a.Exception; } ) );
		}
	}
}