using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Server.Requests.Warmup;

/// <summary>
/// ATTRIBUTION: https://haacked.com/archive/2020/09/28/azure-swap-with-warmup-aspnetcore
/// </summary>
sealed class WarmupAwareHttpsRedirection : ICommand<IApplicationBuilder>
{
	public static WarmupAwareHttpsRedirection Default { get; } = new();

	WarmupAwareHttpsRedirection() : this(IsWarmupRequest.Default.Then().Inverse()) { }

	readonly Func<HttpContext, bool> _condition;

	public WarmupAwareHttpsRedirection(Func<HttpContext, bool> condition) => _condition = condition;

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.UseWhen(_condition, x => x.UseHttpsRedirection());
	}
}