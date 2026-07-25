using DragonSpark.Model.Sequences;
using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

public sealed class Assemblies : Instances<Assembly>
{
	public static Assemblies Default { get; } = new();

	Assemblies() : base(AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName)) {}
}