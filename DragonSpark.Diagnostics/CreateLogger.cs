using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DragonSpark.Diagnostics;

sealed class CreateLogger : IResult<ILogger>
{
    readonly IConfiguration _configuration;

    public CreateLogger(IConfiguration configuration) => _configuration = configuration;

    public ILogger Get() => new LoggerConfiguration().ReadFrom.Configuration(_configuration).CreateLogger();
}