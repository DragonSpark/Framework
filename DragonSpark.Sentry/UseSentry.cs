using DragonSpark.Model.Commands;
using Sentry.AspNetCore;
using Sentry.Extensibility;

namespace DragonSpark.Sentry;

sealed class UseSentry : ICommand<SentryAspNetCoreOptions>
{
	readonly IExceptionFilter _filter;

	public UseSentry(IExceptionFilter filter) => _filter = filter;

	public void Execute(SentryAspNetCoreOptions parameter)
	{
		parameter.AddExceptionFilter(_filter);
		parameter.SetBeforeSend(x => x.Exception is not null ||
		                             x.Message?.Formatted?.Contains("System.OperationCanceledException",
		                                                            StringComparison.Ordinal) != true
			                             ? x
			                             : null);
		parameter.SetBeforeSendLog(x =>
		                           {
			                           var result = !x.TryGetAttribute("sentry.origin", out var origin) ||
			                                        origin is not "auto.log.extensions_logging"
				                                        ? x
				                                        : null;
			                           return result;
		                           });
	}
}