using System;

namespace DragonSpark.Reflection.Assemblies;

public sealed record AssemblyDetails(string Title, string Product, string Company, string Description,
									 string FullName, string Configuration, string Copyright, Version Version);