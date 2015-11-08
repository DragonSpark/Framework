using DragonSpark.Exceptions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Testing.TestObjects
{
	public class TargetObject
	{
		public TargetObjectItem TargetProperty { get; set; }
	}

	public class TargetObjectItem
	{
		public string Name { get; set; }
	}

	public class ExceptionHandler : IExceptionHandler
	{
		public ExceptionHandlingResult Handle( Exception exception )
		{
			return new ExceptionHandlingResult(true, exception);
		}
	}
}
