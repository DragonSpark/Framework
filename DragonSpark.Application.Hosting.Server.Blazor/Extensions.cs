using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using Microsoft.AspNetCore.Builder;
using System;

namespace DragonSpark.Application.Hosting.Server.Blazor;

public static class Extensions
{
	public static ApplicationProfileContext WithBlazorServerApplication(this BuildHostContext @this)
		=> @this.Apply(BlazorApplicationProfile.Default);

	public static ApplicationProfileContext WithBlazorServerApplication(
		this BuildHostContext @this, StaticFileOptions options, Action<IApplicationBuilder> builder)
		=> @this.Apply(new BlazorApplicationProfile(options, builder));
}