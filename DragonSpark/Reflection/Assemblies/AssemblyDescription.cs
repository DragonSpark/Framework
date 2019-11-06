using System.Reflection;

namespace DragonSpark.Reflection.Assemblies
{
	sealed class AssemblyDescription : Declared<AssemblyDescriptionAttribute, string>
	{
		public static AssemblyDescription Default { get; } = new AssemblyDescription();

		AssemblyDescription() : base(x => x.Description) {}
	}
}