using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System.Collections.Generic;
using System.Composition;
using System.Reflection;

namespace DragonSpark.Application
{
	public sealed class ApplicationAssembly : SuppliedSource<IEnumerable<Assembly>, Assembly>
	{
		[Export]
		public static ISource<Assembly> Default { get; } = new ApplicationAssembly();
		ApplicationAssembly() : base( TypeSystem.Configuration.ApplicationAssemblyLocator.ToSourceDelegate(), ApplicationAssemblies.Default.GetEnumerable ) {}
	}
}