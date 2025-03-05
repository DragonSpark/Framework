using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Win32;

namespace DragonSpark.Application.Runtime.Registry;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public sealed class RegisterApplicationProtocol : ICommand<RegisterApplicationProtocolInput>
{
    public static RegisterApplicationProtocol Default { get; } = new();

    RegisterApplicationProtocol() : this(SetKey.Default, DetermineKey.Default) {}

    readonly ICommand<SetKeyInput>                  _set;
    readonly ISelect<RegistryKeyInput, RegistryKey> _key;

    public RegisterApplicationProtocol(ICommand<SetKeyInput> set, ISelect<RegistryKeyInput, RegistryKey> key)
    {
        _set = set;
        _key = key;
    }

    public void Execute(RegisterApplicationProtocolInput parameter)
    {
        var (moniker, name, location) = parameter;

        var path = $"Software/Classes/{moniker}";
        _key.Get(new(path, $"URL:{name}"));
        _set.Execute(new(path, "URL Protocol", string.Empty));
        _key.Get(new($"{path}/DefaultIcon", $"{location},1"));
        _key.Get(new($"{path}/shell/open/command", $@"""{location}"" ""%1"""));
    }
}