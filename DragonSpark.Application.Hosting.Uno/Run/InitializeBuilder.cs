using DragonSpark.Application.Mobile.Presentation;
using DragonSpark.Application.Mobile.Run;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

sealed class InitializeBuilder(Func<IHostBuilder, IHostBuilder> host, Action<IApplicationBuilder> configure)
	: ISelect<InitializeInput, IApplicationBuilder>
{
	public IApplicationBuilder Get(InitializeInput parameter)
	{
		var (owner, arguments) = parameter;
		var builder = (IApplicationBuilder)new Builder(owner.CreateBuilder(arguments), host);
		var result  = owner is IApplication application ? new ConfigureAwareBuilder(builder, application) : builder;
		configure(result);
		return result;
	}
}
