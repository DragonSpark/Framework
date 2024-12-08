using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Composition.Compose;
using Microsoft.AspNetCore.Builder;
using System;
using System.Reflection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

public static class Extensions
{
	public static ApplicationProfileContext WithBlazorServerApplication(this BuildHostContext @this)
		=> @this.Apply(BlazorApplicationProfile.Default);

	public static ApplicationProfileContext WithBlazorServerApplication(
		this BuildHostContext @this, Action<IApplicationBuilder> builder)
		=> @this.Apply(new BlazorApplicationProfile(builder));

	public static ApplicationProfileContext WithBlazorServerApplication<T>(
		this BuildHostContext @this, params Assembly[] additional)
		=> @this.WithBlazorServerApplication<T>(_ => {}, additional);

	public static ApplicationProfileContext WithBlazorServerApplication<T>(
		this BuildHostContext @this, Action<IApplicationBuilder> builder, params Assembly[] additional)
		=> @this.Apply(new BlazorApplicationProfile<T>(builder, additional));

}