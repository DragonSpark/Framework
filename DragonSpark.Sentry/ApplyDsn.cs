using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Sentry;

sealed class ApplyDsn : ICommand<ApplyDsnInput>
{
	public static ApplyDsn Default { get; } = new();

	ApplyDsn() {}

	public void Execute(ApplyDsnInput parameter)
	{
		var (configuration, name) = parameter;
		if (name.IsAssigned())
		{
			var named = configuration[$"Sentry:dsn:{name}"];
			if (named.IsAssigned())
			{
				configuration["Sentry:dsn"] = named;
			}
		}
	}
}