using System;
using System.Reflection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Composition;

sealed record Modularity(Array<Assembly> Assemblies, Array<Type> Types, IComponentTypes ComponentTypes,
                         IComponentType ComponentType)
{
    public Modularity(Array<Assembly> Assemblies, Array<Type> Types, IComponentTypes ComponentTypes)
        : this(Assemblies, Types, ComponentTypes, new ComponentType(ComponentTypes)) {}
}