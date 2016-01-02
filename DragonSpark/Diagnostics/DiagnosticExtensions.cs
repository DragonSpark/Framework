using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Diagnostics
{
	public static class DiagnosticExtensions
	{
		public static Exception Try( this Action action )
		{
			var result = Activator.Current.Activate<TryContext>().Try( action );
			return result;
		}

		public static void Process( this IExceptionHandler target, Exception exception )
		{
			target.Handle( exception ).With( a => a.RethrowRecommended.IsTrue( () => { throw a.Exception; } ) );
		}
	}
}