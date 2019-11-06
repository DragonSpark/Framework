using System.Reflection;

namespace DragonSpark.Reflection.Assemblies
{
	sealed class AssemblyCompany : Declared<AssemblyCompanyAttribute, string>
	{
		public static AssemblyCompany Default { get; } = new AssemblyCompany();

		AssemblyCompany() : base(x => x.Company) {}
	}
}