using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class Configure : ICommand<IApplicationBuilder>
{
    public static Configure Default { get; } = new();

    Configure() {}

    public void Execute(IApplicationBuilder parameter)
    {
        parameter.UseMiddleware<PasskeyResponseInterceptionMiddleware>();
    }
}