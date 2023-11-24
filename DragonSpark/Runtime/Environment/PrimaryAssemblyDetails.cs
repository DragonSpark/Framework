﻿using DragonSpark.Model.Results;
using DragonSpark.Reflection.Assemblies;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

public sealed class PrimaryAssemblyDetails : FixedSelectedSingleton<Assembly, AssemblyDetails>
{
	public static PrimaryAssemblyDetails Default { get; } = new();

	PrimaryAssemblyDetails() : base(GetAssemblyDetails.Default, PrimaryAssembly.Default) {}
}