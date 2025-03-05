using Microsoft.Win32;

namespace DragonSpark.Application.Runtime.Registry;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public readonly record struct RegistryKeyInput(RegistryKey Root, string Path, string? DefaultValue = null)
{
    public RegistryKeyInput(string Path, string? DefaultValue = null) :
        this(Microsoft.Win32.Registry.CurrentUser, Path, DefaultValue) {}
}