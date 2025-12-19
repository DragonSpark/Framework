using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Server.Requests.Warmup;

sealed class IsDeployedEnvironment : Condition<HttpContext>
{
    public static IsDeployedEnvironment Default { get; } = new();

    IsDeployedEnvironment() : base(x => !x.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment()) {}
}