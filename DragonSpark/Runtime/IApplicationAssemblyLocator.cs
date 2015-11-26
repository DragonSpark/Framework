using DragonSpark.Activation;
using DragonSpark.Setup;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Runtime
{
	public interface IApplicationAssemblyLocator : IFactory<Assembly>
	{}

	[AttributeUsage( AttributeTargets.Assembly )]
	public class ApplicationAttribute : Attribute
	{}

	[RegisterFactoryForResult]
	public class AssembliesFactory : FactoryBase<Assembly[]>
	{
		readonly IAssemblyProvider provider;

		public AssembliesFactory( IAssemblyProvider provider )
		{
			this.provider = provider;
		}

		protected override Assembly[] CreateItem()
		{
			return provider.GetAssemblies();
		}
	}

	public class ApplicationAssemblyLocator : FactoryBase<Assembly>, IApplicationAssemblyLocator
	{
		readonly Assembly[] assemblies;

		public ApplicationAssemblyLocator( Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		protected override Assembly CreateItem()
		{
			var result = assemblies.SingleOrDefault( assembly => assembly.GetCustomAttribute<ApplicationAttribute>() != null );
			return result;
		}
	}
}