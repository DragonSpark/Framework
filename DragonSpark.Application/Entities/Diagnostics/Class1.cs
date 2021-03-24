using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using Polly;
using System;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Entities.Diagnostics
{
	class Class1
	{
	}

	public sealed class ApplicationContentPolicy : DragonSpark.Diagnostics.Policy
	{
		public static ApplicationContentPolicy Default { get; } = new ApplicationContentPolicy();

		ApplicationContentPolicy()
			: base(Policy.Handle<InvalidOperationException>(IsApplicationContentException.Default.Get)) {}
	}

	sealed class IsApplicationContentException : ICondition<InvalidOperationException>
	{
		public static IsApplicationContentException Default { get; } = new IsApplicationContentException();

		IsApplicationContentException()
			: this("A second operation was started on this context before a previous operation completed. This is usually caused by different threads concurrently using the same instance of DbContext.") {}

		readonly string _message;

		public IsApplicationContentException(string message) => _message = message;

		public bool Get(InvalidOperationException parameter) => parameter.Message.StartsWith(_message);
	}

	public sealed class DurableApplicationContentPolicy : DeferredSingleton<IAsyncPolicy>
	{
		public static DurableApplicationContentPolicy Default { get; } = new DurableApplicationContentPolicy();

		DurableApplicationContentPolicy()
			: base(ApplicationContentPolicy.Default.Then().Select(ApplicationContentRetryPolicy.Default)) {}
	}

	public sealed class ApplicationContentRetryPolicy : RetryPolicy
	{
		public static ApplicationContentRetryPolicy Default { get; } = new ApplicationContentRetryPolicy();

		ApplicationContentRetryPolicy() : base(10, new LinearRetryStrategy(TimeSpan.FromMilliseconds(50)).Get) {}
	}


}
