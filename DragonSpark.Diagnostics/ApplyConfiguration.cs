using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace DragonSpark.Diagnostics;

sealed class ApplyConfiguration : ICommand<ApplyConfigurationInput>
{
	public static ApplyConfiguration Default { get; } = new();

	ApplyConfiguration() : this(LoggingSectionName.Default) {}

	readonly string _name;

	public ApplyConfiguration(string name) => _name = name;

	public void Execute(ApplyConfigurationInput parameter)
	{
		var (subject, configuration) = parameter;

		var filter = new ReloadableForwardedFilter(configuration);
		ChangeToken.OnChange(configuration.GetSection(_name).GetReloadToken, filter.Execute);
		subject.ReadFrom.Configuration(configuration).Filter.With(filter);
	}
}