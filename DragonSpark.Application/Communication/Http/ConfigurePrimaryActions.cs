namespace DragonSpark.Application.Communication.Http;

sealed class ConfigurePrimaryActions : ConfigureActions
{
    public static ConfigurePrimaryActions Default { get; } = new();

    ConfigurePrimaryActions() : base(AddPrimaryAction.Default.Execute) {}
}