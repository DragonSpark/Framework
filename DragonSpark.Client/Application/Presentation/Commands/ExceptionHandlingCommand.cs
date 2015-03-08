using System;
using DragonSpark.IoC;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Commands
{
	[Singleton]
	public class ExceptionHandlingCommand : CommandBase<Exception>
	{
		readonly IExceptionHandler handler;

		public ExceptionHandlingCommand( IExceptionHandler handler )
		{
			this.handler = handler;
		}

		protected override void Execute( Exception parameter )
		{
			handler.Handle( parameter );
		}
	}
}