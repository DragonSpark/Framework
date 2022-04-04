using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server;

public static class Extension
{
	public static ApplicationProfileContext WithServerApplication(this BuildHostContext @this)
		=> @this.Apply(ServerApplicationProfile.Default);

	public static ApplicationProfileContext WithServerApplication(this BuildHostContext @this,
	                                                              ICommand<IApplicationBuilder> application)
		=> @this.Apply(new ServerApplicationProfile(application));
}