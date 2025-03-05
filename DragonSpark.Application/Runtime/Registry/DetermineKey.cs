using System;
using DragonSpark.Model.Selection;
using Microsoft.Win32;

namespace DragonSpark.Application.Runtime.Registry;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
sealed class DetermineKey : ISelect<RegistryKeyInput, RegistryKey>
{
    public static DetermineKey Default { get; } = new();

    DetermineKey() {}

    public RegistryKey Get(RegistryKeyInput parameter)
    {
        var (root, path, defaultValue) = parameter;

        var result = root;

        foreach (var key in path.Split('/'))
        {
            result = result.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree)
                     ?? result.CreateSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                     throw new InvalidOperationException("Could not get or create key");
        }

        if (defaultValue is not null)
        {
            result.SetValue(string.Empty, defaultValue);
        }

        return result;
    }
}