using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Assemblies;

namespace DragonSpark.Runtime.Environment
{
	sealed class Assemblies : ArrayResult<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : base(A.This(PrimaryAssembly.Default)
		                     .Select(AssemblyNameSelector.Default)
		                     .Select(ComponentAssemblyNames.Default)
		                     .Query()
		                     .Select(Load.Default)
		                     .Append(Sequence.Using(HostingAssembly.Default))
		                     .WhereBy(y => y != null)
		                     .Distinct()
		                     .Selector()
		                     .Start()) {}
	}
}