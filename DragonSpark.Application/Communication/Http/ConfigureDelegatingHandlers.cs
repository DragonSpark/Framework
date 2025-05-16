namespace DragonSpark.Application.Communication.Http;

sealed class ConfigureDelegatingHandlers : ConfigureActions
{
    public static ConfigureDelegatingHandlers Default { get; } = new();

    ConfigureDelegatingHandlers() : base(AddAction.Default.Execute) {}
}