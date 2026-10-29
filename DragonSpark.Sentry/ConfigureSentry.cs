using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sentry.AspNetCore;

namespace DragonSpark.Sentry;

sealed class ConfigureSentry : ICommand<IHostApplicationBuilder>
{
	readonly string?                         _name;
	readonly Action<SentryAspNetCoreOptions> _use;
	readonly ICommand<ApplyDsnInput>         _apply;

	public ConfigureSentry(string? name) : this(name, UseSentry.Default.Execute, ApplyDsn.Default) {}

	public ConfigureSentry(string? name, Action<SentryAspNetCoreOptions> use, ICommand<ApplyDsnInput> apply)
	{
		_name  = name;
		_use   = use;
		_apply = apply;
	}

	public void Execute(IHostApplicationBuilder parameter)
	{
		_apply.Execute(new(parameter.Configuration, _name));

		parameter.Services.GetRequiredInstance<WebApplicationBuilder>().WebHost.UseSentry(_use);
	}
}