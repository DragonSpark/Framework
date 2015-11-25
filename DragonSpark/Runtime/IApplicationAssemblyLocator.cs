using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.Setup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Runtime
{
	public interface IApplicationAssemblyLocator
	{
		Assembly Locate();
	}

	[AttributeUsage( AttributeTargets.Assembly )]
	public class ApplicationAttribute : Attribute
	{}

	[RegisterResultType]
	public class AssembliesFactory : Factory<Assembly[]>
	{
		readonly IAssemblyProvider provider;

		public AssembliesFactory( IAssemblyProvider provider ) : this( provider, Activator.Current )
		{}

		public AssembliesFactory( IAssemblyProvider provider, IActivator activator ) : base( activator )
		{
			this.provider = provider;
		}

		protected override Assembly[] CreateFrom( object parameter )
		{
			return provider.GetAssemblies();
		}
	}

	public class ApplicationAssemblyLocator : IApplicationAssemblyLocator
	{
		readonly Assembly[] assemblies;

		public ApplicationAssemblyLocator( Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		public Assembly Locate()
		{
			var result = assemblies.SingleOrDefault( assembly => assembly.GetCustomAttribute<ApplicationAttribute>() != null );
			return result;
		}
	}
}