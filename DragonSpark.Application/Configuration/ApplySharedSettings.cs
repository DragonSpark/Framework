using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Configuration;

sealed class ApplySharedSettings : ICommand<(HostBuilderContext Context, IConfigurationBuilder Builder)>
{
	public static ApplySharedSettings Default { get; } = new();

	ApplySharedSettings() : this(SharedSettingsSources.Default) {}

	readonly SharedSettingsSources _sources;

	public ApplySharedSettings(SharedSettingsSources sources) => _sources = sources;

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