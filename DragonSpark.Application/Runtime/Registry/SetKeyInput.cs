using Microsoft.Win32;

namespace DragonSpark.Application.Runtime.Registry;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public readonly record struct SetKeyInput(RegistryKey Root, string Path, string Name, string Value)
{
    public SetKeyInput(string Path, string Name, string Value) : this(Microsoft.Win32.Registry.CurrentUser, Path, Name, Value) {}
}