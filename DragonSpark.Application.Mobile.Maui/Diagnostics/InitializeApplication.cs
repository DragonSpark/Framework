using DragonSpark.Application.Mobile.Runtime.Initialization;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public sealed class InitializeApplication : InitializationAware
{
    public InitializeApplication(IReport send) : base(PerformInitialization.Default, send) {}
}