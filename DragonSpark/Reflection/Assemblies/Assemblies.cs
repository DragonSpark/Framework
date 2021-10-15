using DragonSpark.Model.Sequences;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

public sealed class Assemblies : Instances<Assembly>
{
	public static Assemblies Default { get; } = new Assemblies();

	Assemblies() : base(AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName)) {}
}