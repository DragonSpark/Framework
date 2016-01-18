using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using PostSharp.Patterns.Contracts;
using System.Linq;
using System.Reflection;
using DragonSpark.Runtime.Values;

namespace DragonSpark.TypeSystem
{
	public static class Assemblies
	{
		public static Assembly[] GetCurrent() => Services.Locate<Assembly[]>() ?? new AssemblyHost().Item ?? Default<Assembly>.Items;

		public delegate Assembly[] Get();
	}

	public class AssemblyHost : ExecutionContextValue<Assembly[]> {}

	public class ApplicationAssemblyLocator : FactoryBase<Assembly>, IApplicationAssemblyLocator
	{
		readonly Assembly[] assemblies;

		public ApplicationAssemblyLocator( [Required]Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		protected override Assembly CreateItem() => assemblies.SingleOrDefault( assembly => assembly.GetCustomAttribute<ApplicationAttribute>() != null );
	}
}