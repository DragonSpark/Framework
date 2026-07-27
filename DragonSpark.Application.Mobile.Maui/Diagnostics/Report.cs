using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class Report<T> : IReport<T>
{
    readonly Func<T, IConfiguration> _configuration;

    public Report(Func<T, IConfiguration> configuration) => _configuration = configuration;

    public void Execute(SendExceptionInput<T> parameter)
    {
        var (input, exception) = parameter;
        var address = _configuration(input).Section<InitializationLoggingSettings>() is
                          { Enabled: true, Address: not null and not "" } s
                          ? s.Address
                          : null;
        if (address is not null)
        {
            using var _ = SentrySdk.Init(address);
            SentrySdk.CaptureException(exception);
            SentrySdk.Flush(TimeSpan.FromSeconds(4));
        }
    }
}

sealed class Report : IReport
{
    readonly ICommand<SendExceptionInput<None>> _previous;

    public Report(IConfiguration configuration) : this(new Report<None>(configuration.Accept)) {}

    public Report(ICommand<SendExceptionInput<None>> previous) => _previous = previous;

    public void Execute(Exception parameter)
    {
        _previous.Execute(new(None.Default, parameter));
    }
}