using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Configuration;

sealed class ApplyAmbientConfiguration : ICommand<(HostBuilderContext Context, IConfigurationBuilder Builder)>
{
	public static ApplyAmbientConfiguration Default { get; } = new();

	ApplyAmbientConfiguration() : this(AmbientConfigurationSources.Default) {}

	readonly AmbientConfigurationSources _sources;

	public ApplyAmbientConfiguration(AmbientConfigurationSources sources) => _sources = sources;

	public void Execute((HostBuilderContext Context, IConfigurationBuilder Builder) parameter)
	{
		var (context, builder) = parameter;
		using var sources = _sources.Get(context.HostingEnvironment);
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