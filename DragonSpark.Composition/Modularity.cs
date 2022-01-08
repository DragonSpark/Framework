using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Environment;
using System;
using System.Reflection;

namespace DragonSpark.Composition;

sealed record Modularity(Array<Assembly> Assemblies, Array<Type> Types, IComponentTypes ComponentTypes,
                         IComponentType ComponentType);