using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

sealed class AssemblyCreatedTime : Select<Assembly, DateTimeOffset>
{
	public static AssemblyCreatedTime Default { get; } = new();

	AssemblyCreatedTime() : base(x => x.Attribute<AssemblyBuildDateAttribute>().Verify().On) {}
}