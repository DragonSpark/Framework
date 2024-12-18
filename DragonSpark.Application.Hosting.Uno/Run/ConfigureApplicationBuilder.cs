using DragonSpark.Application.Mobile.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Hosting.Uno.Run;

sealed class ConfigureApplicationBuilder : Command<Mobile.Run.Application>, IConfigureApplicationBuilder
{
	readonly Type _key;

	public ConfigureApplicationBuilder(IApplication subject) : this(subject, A.Type<ConfigureApplicationBuilder>()) {}

	public ConfigureApplicationBuilder(IApplication subject, Type key) : base(subject) => _key = key;

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.Properties[_key] = this;
	}
}
