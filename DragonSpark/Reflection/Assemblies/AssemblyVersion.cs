using System;
using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

sealed class AssemblyVersion : Declared<AssemblyFileVersionAttribute, Version>
{
	public static AssemblyVersion Default { get; } = new AssemblyVersion();

	AssemblyVersion() : base(x => Version.Parse(x.Version)) {}
}