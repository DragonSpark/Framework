using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Application.Entities.Diagnostics
{
	sealed class IsApplicationContentException : ICondition<InvalidOperationException>
	{
		public static IsApplicationContentException Default { get; } = new IsApplicationContentException();

		IsApplicationContentException()
			: this("A second operation was started on this context before a previous operation completed. This is usually caused by different threads concurrently using the same instance of DbContext.") {}

		readonly string _message;

		public IsApplicationContentException(string message) => _message = message;

		public bool Get(InvalidOperationException parameter) => parameter.Message.StartsWith(_message);
	}
}