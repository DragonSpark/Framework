using DragonSpark.Application.Mobile.Runtime.Initialization;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public sealed class InitializeApplication : InitializationAware
{
    public InitializeApplication(IConfiguration configuration) : base(configuration, PerformInitialization.Default) {}
}