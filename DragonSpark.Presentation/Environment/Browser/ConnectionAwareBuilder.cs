using DragonSpark.Diagnostics;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Policy = Polly.Policy;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class ConnectionAwareBuilder : Builder
{
	public static ConnectionAwareBuilder Default { get; } = new();

	ConnectionAwareBuilder() : this(IsInteropException.Default.Get) {}

	ConnectionAwareBuilder(Func<Exception, bool> message)
		: base(Policy.Handle<JSDisconnectedException>()
		             .Or<TaskCanceledException>()
		             .Or<InvalidOperationException>(message)) {}
}

sealed class ConnectionAwareBuilder<T> : Builder<T>
{
	public static ConnectionAwareBuilder<T> Default { get; } = new();

	ConnectionAwareBuilder() : this(IsInteropException.Default.Get) {}

	ConnectionAwareBuilder(Func<Exception, bool> message)
		: base(Polly.Policy<T>.Handle<JSDisconnectedException>()
		            .Or<TaskCanceledException>()
		            .Or<InvalidOperationException>(message)) {}
}