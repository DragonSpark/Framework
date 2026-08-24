using DragonSpark.Composition;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.AspNet.Security.Identity.State;

sealed class ApplyServerSettings<T> : ISelect<IServiceCollection, IDataProtectionBuilder> where T : SystemServerSettings
{
	public static ApplyServerSettings<T> Default { get; } = new();

	ApplyServerSettings() : this(SharedAccessStateKeyName.Default) {}

	readonly string _name;

	public ApplyServerSettings(string name) => _name = name;

	public IDataProtectionBuilder Get(IServiceCollection parameter)
	{
		var result = parameter.Register<T>().AddDataProtection();
		parameter.AddOptions<DataProtectionOptions>()
		         .Configure<T, IHostEnvironment>((options, settings, env) =>
		                                         {
			                                         options.ApplicationDiscriminator =
				                                         $"{settings.Name}-{_name}-{env.EnvironmentName}";
		                                         })
		         .Services.AddOptions<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme)
		         .Configure<T>((options, settings) => options.Cookie.Domain = settings.Domain);
		return result;
	}
}