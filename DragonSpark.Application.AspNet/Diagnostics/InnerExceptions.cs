using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.AspNet.Diagnostics;

sealed class InnerExceptions : ISelect<Exception, InnerExceptions.InnerExceptionsEnumerable>
{
	public static InnerExceptions Default { get; } = new();

	InnerExceptions() {}

	public InnerExceptionsEnumerable Get(Exception parameter) => new(parameter);

	public readonly struct InnerExceptionsEnumerable
	{
		readonly Exception _exception;

		public InnerExceptionsEnumerable(Exception exception) => _exception = exception;

		public InnerExceptionsEnumerator GetEnumerator() => new(_exception);
	}

	public struct InnerExceptionsEnumerator
	{
		Exception? _current;

		public InnerExceptionsEnumerator(Exception exception) => _current = exception;

		public Exception Current => _current ?? throw new InvalidOperationException();

		public bool MoveNext()
		{
			if (_current is not null)
			{
				_current = _current.InnerException;
				return _current is not null;
			}

			return false;
		}
	}
}