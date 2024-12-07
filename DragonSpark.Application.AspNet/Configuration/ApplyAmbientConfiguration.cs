using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Configuration;

public sealed class ApplyAmbientConfiguration : ICommand<(IHostEnvironment Environment, IConfigurationBuilder Builder)>
{
	public static ApplyAmbientConfiguration Default { get; } = new();

	ApplyAmbientConfiguration() : this(AmbientConfigurationSources.Default) {}

	readonly ISelect<IHostEnvironment, Leasing<JsonConfigurationSource>> _sources;

	public ApplyAmbientConfiguration(ISelect<IHostEnvironment, Leasing<JsonConfigurationSource>> sources)
		=> _sources = sources;

	public void Execute(HostBuilderContext context, IConfigurationBuilder builder)
	{
		Execute((context.HostingEnvironment, builder));
	}

	public void Execute((IHostEnvironment Environment, IConfigurationBuilder Builder) parameter)
	{
		var (environment, builder) = parameter;
		using var sources = _sources.Get(environment);
		if (builder.Sources.Count > 0)
		{
			var span = sources.AsSpan();
			for (var i = 0; i < sources.Length; i++)
			{
				span[i].ResolveFileProvider();
				builder.Sources.Insert(i, span[i]);
			}
		}
		else
		{
			foreach (var source in sources)
			{
				source.ResolveFileProvider();
				builder.Add(source);
			}
		}
	}
}