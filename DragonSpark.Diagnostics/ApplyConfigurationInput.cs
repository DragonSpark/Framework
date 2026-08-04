using Microsoft.Extensions.Configuration;
using Serilog;

namespace DragonSpark.Diagnostics;

public readonly record struct ApplyConfigurationInput(LoggerConfiguration Subject, IConfiguration Configuration)
{
	public ApplyConfigurationInput(IConfiguration Configuration) : this(new(), Configuration) {}
}