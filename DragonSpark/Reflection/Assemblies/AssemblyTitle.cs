using System.Reflection;

namespace DragonSpark.Reflection.Assemblies
{
	sealed class AssemblyTitle : Declared<AssemblyTitleAttribute, string>
	{
		public static AssemblyTitle Default { get; } = new AssemblyTitle();

		AssemblyTitle() : base(x => x.Title) {}
	}
}