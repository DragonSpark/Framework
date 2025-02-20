using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using DragonSpark.Text;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition;

sealed class DetermineModularityInput : ISelect<HostBuilderContext, ModularityInput>
{
    public static DetermineModularityInput Default { get; } = new();

    DetermineModularityInput() : this(GetHostEnvironmentName.Default) {}

    readonly IFormatter<HostBuilderContext> _environment;

    public DetermineModularityInput(IFormatter<HostBuilderContext> environment) => _environment = environment;

    public ModularityInput Get(HostBuilderContext parameter)
    {
        var platform    = new AccessPlatform(parameter).Get()?.To<string>();
        var environment = _environment.Get(parameter);
        return new(platform, environment);
    }
}