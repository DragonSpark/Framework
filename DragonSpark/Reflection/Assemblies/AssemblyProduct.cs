using System.Reflection;

namespace DragonSpark.Reflection.Assemblies;

sealed class AssemblyProduct : Declared<AssemblyProductAttribute, string>
{
	public static AssemblyProduct Default { get; } = new AssemblyProduct();

	AssemblyProduct() : base(x => x.Product) {}
}