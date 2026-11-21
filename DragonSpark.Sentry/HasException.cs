using DragonSpark.Model.Selection.Conditions;
using Sentry.Protocol;
using System.Buffers;

namespace DragonSpark.Sentry;

sealed class HasException : ICondition<SentryEvent>
{
	public static HasException Default { get; } = new();

	HasException() : this(ShouldIgnoreSentryException.Default.Get, IgnoredExceptionTypes.Default) {}

	readonly Func<SentryException, bool> _ignore;
	readonly SearchValues<string>        _types;

	public HasException(Func<SentryException, bool> ignore, SearchValues<string> types)
	{
		_ignore = ignore;
		_types  = types;
	}

	public bool Get(SentryEvent parameter)
		=> parameter.Exception is not null
		   && parameter.SentryExceptions?.Any(_ignore) != true
		   && (!parameter.Extra.TryGetValue("error.type", out var error)
		       || error?.ToString() is not {} type
		       || !_types.Contains(type));
}