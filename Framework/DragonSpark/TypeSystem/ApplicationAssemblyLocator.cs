using System.Linq;
using System.Reflection;
using DragonSpark.Activation.Build;

namespace DragonSpark.TypeSystem
{
	public class ApplicationAssemblyLocator : FactoryBase<Assembly>, IApplicationAssemblyLocator
	{
		readonly Assembly[] assemblies;

		public ApplicationAssemblyLocator( Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		protected override Assembly CreateItem()
		{
			var result = assemblies.SingleOrDefault( assembly => CustomAttributeExtensions.GetCustomAttribute<ApplicationAttribute>( (Assembly)assembly ) != null );
			return result;
		}
	}
}