using DragonSpark.Model.Commands;
using Sentry.AspNetCore;

namespace DragonSpark.Sentry;

sealed class UseSentry : ICommand<SentryAspNetCoreOptions>
{
	public static UseSentry Default { get; } = new();

	UseSentry() : this(Process.Default.Get) {}

	readonly Func<SentryEvent, SentryEvent?> _select;

	public UseSentry(Func<SentryEvent, SentryEvent?> select) => _select = select;

	public void Execute(SentryAspNetCoreOptions parameter)
	{
		parameter.SetBeforeSend(_select);
		parameter.SetBeforeSendLog(x =>
		                           {
			                           var result = !x.TryGetAttribute("sentry.origin", out var origin) ||
			                                           origin is not "auto.log.extensions_logging"
				                                           ? x
				                                           : null;
			                           return result;
		                           });
	}}