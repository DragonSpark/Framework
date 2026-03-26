using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearDeviceState : IStopAware
{
    public static ClearDeviceState Default { get; } = new();

    ClearDeviceState() {}

    public ValueTask Get(CancellationToken parameter) => ValueTask.CompletedTask;
}