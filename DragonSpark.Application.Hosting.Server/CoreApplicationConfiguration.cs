using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Server.Requests.Warmup;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server;

sealed class CoreApplicationConfiguration : IAlteration<IApplicationBuilder>
{
    public static CoreApplicationConfiguration Default { get; } = new();

    CoreApplicationConfiguration() {}

    public IApplicationBuilder Get(IApplicationBuilder parameter)
        => parameter.UseWarmupAwareHttpsRedirection().UseAuthentication().UseRouting().UseAuthorization();
}