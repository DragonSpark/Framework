using DragonSpark.Model.Selection.Conditions;
using Sentry.Protocol;
using System.Buffers;

namespace DragonSpark.Sentry;

sealed class ShouldIgnoreSentryException : ICondition<SentryException>
{
	public static ShouldIgnoreSentryException Default { get; } = new();

	ShouldIgnoreSentryException() : this(IgnoredExceptionTypes.Default) {}

	readonly SearchValues<string> _types;

	public ShouldIgnoreSentryException(SearchValues<string> types) => _types = types;

	public bool Get(SentryException parameter) => parameter.Type is not null && _types.Contains(parameter.Type);
}