using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;

namespace DragonSpark.Runtime.Environment;

sealed class ComponentAssemblyNames : ISelect<AssemblyName, IEnumerable<AssemblyName>>
{
    readonly Func<AssemblyName, IEnumerable<AssemblyName>> _expand;
    readonly Array<IAlteration<AssemblyName>>              _names;

    public ComponentAssemblyNames(ModularityInput input) : this(DetermineAssemblyNames.Default.Get(input)) {}

    public ComponentAssemblyNames(params IAlteration<AssemblyName>[] names)
        : this(ComponentAssemblyCandidates.Default.Get, names) {}

    public ComponentAssemblyNames(Func<AssemblyName, IEnumerable<AssemblyName>> expand,
                                  Array<IAlteration<AssemblyName>> names)
    {
        _expand = expand;
        _names  = names;
    }

    public IEnumerable<AssemblyName> Get(AssemblyName parameter)
    {
        foreach (var name in _expand(parameter).AsValueEnumerable())
        {
            foreach (var alteration in _names.Open())
            {
                yield return alteration.Get(name);
            }
        }
    }
}