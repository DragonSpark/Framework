using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Win32;

namespace DragonSpark.Application.Runtime.Registry;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
sealed class SetKey : ICommand<SetKeyInput>
{
    public static SetKey Default { get; } = new();

    SetKey() : this(DetermineKey.Default) {}

    readonly ISelect<RegistryKeyInput, RegistryKey> _key;

    public SetKey(ISelect<RegistryKeyInput, RegistryKey> key) => _key = key;

    public void Execute(SetKeyInput parameter)
    {
        var (root, path, name, value) = parameter;

        _key.Get(new(root, path)).SetValue(name, value);
    }
}