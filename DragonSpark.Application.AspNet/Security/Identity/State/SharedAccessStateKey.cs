using DragonSpark.Text;
using Humanizer;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.AspNet.Security.Identity.State;

public abstract class SharedAccessStateKey<T> : IFormatter<IHostEnvironment> where T : SystemServerSettings
{
	readonly Func<SystemServerSettings> _settings;
	readonly string                     _template;

	protected SharedAccessStateKey(Func<T> settings)
		: this(settings, $"{{0}}-{SharedAccessStateKeyName.Default}-{{1}}") {}

	protected SharedAccessStateKey(Func<SystemServerSettings> settings, string template)
	{
		_settings = settings;
		_template = template;
	}

	public string Get(IHostEnvironment parameter) => _template.FormatWith(_settings().Name, parameter.EnvironmentName);
}