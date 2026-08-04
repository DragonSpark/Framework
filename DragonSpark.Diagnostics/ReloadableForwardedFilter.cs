using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace DragonSpark.Diagnostics;

sealed class ReloadableForwardedFilter : Variable<ILogEventFilter>, ILogEventFilter, ICommand
{
	readonly IConfiguration _configuration;

	public ReloadableForwardedFilter(IConfiguration configuration)
		: this(configuration, new ForwardedFilter(configuration)) {}

	public ReloadableForwardedFilter(IConfiguration configuration, ILogEventFilter instance) : base(instance)
		=> _configuration = configuration;

	public void Execute(None parameter)
	{
		Execute(new ForwardedFilter(_configuration));
	}

	public bool IsEnabled(LogEvent logEvent) => Get().Verify().IsEnabled(logEvent);
}