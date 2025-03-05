using System;
using DragonSpark.Compose;
using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Runtime.Registry;
/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
/// <param name="Moniker"></param>
/// <param name="Name"></param>
/// <param name="Location"></param>
public readonly record struct RegisterApplicationProtocolInput(string Moniker, string Name, string Location)
{
    public RegisterApplicationProtocolInput(string moniker)
        : this(moniker, PrimaryAssemblyDetails.Default, Environment.ProcessPath.Verify()) {}

    public RegisterApplicationProtocolInput(string moniker, AssemblyDetails details, string location)
        : this(moniker, details.Company, location) {}
}