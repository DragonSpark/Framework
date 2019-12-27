﻿namespace DragonSpark.Diagnostics.Logging
{
	public readonly struct ExceptionParameter<T>
	{
		public ExceptionParameter(System.Exception exception, T argument)
		{
			Exception = exception;
			Argument  = argument;
		}

		public System.Exception Exception { get; }

		public T Argument { get; }
	}
}