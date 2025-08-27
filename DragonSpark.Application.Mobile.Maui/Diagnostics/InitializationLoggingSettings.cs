namespace DragonSpark.Application.Mobile.Maui.Diagnostics;
public sealed record InitializationLoggingSettings
{
    public bool Enabled { get; set; } = true;

    public string Address { get; set; } = string.Empty;
}