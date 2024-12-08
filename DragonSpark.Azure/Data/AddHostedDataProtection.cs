using DragonSpark.Application.AspNet.Security.Identity.State;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Data;

public abstract class AddHostedDataProtection(IAlteration<IDataProtectionBuilder> configure)
	: IAlteration<IServiceCollection>
{
	readonly IAlteration<IDataProtectionBuilder> _configure = configure;

	protected AddHostedDataProtection(SetApplicationName name) : this(name.Then().Select(HostedKeys.Default).Out()) {}

	public IServiceCollection Get(IServiceCollection parameter)
		=> _configure.Get(parameter.AddDataProtection()).Services;
}