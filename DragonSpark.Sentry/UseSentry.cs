using DragonSpark.Model.Commands;
using Sentry.AspNetCore;
using Sentry.Extensibility;

namespace DragonSpark.Sentry;

sealed class UseSentry : ICommand<SentryAspNetCoreOptions>
{
	readonly IExceptionFilter                _filter;
	readonly Func<SentryEvent, SentryEvent?> _before;

	public UseSentry(IExceptionFilter filter) : this(filter, BeforeSend.Default.Get) {}

	public UseSentry(IExceptionFilter filter, Func<SentryEvent, SentryEvent?> before)
	{
		_filter = filter;
		_before = before;
	}

	public void Execute(SentryAspNetCoreOptions parameter)
	{
		parameter.AddExceptionFilter(_filter);
		parameter.SetBeforeSend(_before);
		parameter.SetBeforeSendLog(x => !x.TryGetAttribute("sentry.origin", out var origin)
		                                || origin is not "auto.log.extensions_logging"
			                                ? x
			                                : null);
	}
}