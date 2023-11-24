using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

sealed class AssemblyCopyright : Declared<AssemblyCopyrightAttribute, string>
{
	public static AssemblyCopyright Default { get; } = new();

	AssemblyCopyright() : base(x => x.Copyright) {}
}