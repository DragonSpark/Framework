using Azure;
using DragonSpark.Diagnostics;

namespace DragonSpark.Azure.Storage;

sealed class RequestBuilder<T> : Builder<T>
{
	public static RequestBuilder<T> Default { get; } = new();

	RequestBuilder() : base(Polly.Policy<T>.Handle<RequestFailedException>()) {}
}