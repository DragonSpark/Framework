using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Application
{
	public sealed class ApplicationAssemblyLocator : ParameterizedSourceBase<IEnumerable<Assembly>, Assembly>
	{
		readonly static Func<Assembly, bool> Specification = ApplicationAssemblySpecification.Default.ToDelegate();
		
		public static ApplicationAssemblyLocator Default { get; } = new ApplicationAssemblyLocator();
		ApplicationAssemblyLocator() : this( Specification ) {}

		readonly Func<Assembly, bool> specification;
		
		public ApplicationAssemblyLocator( Func<Assembly, bool> specification )
		{
			this.specification = specification;
		}

		public override Assembly Get( IEnumerable<Assembly> parameter ) => parameter.Only( specification );
	}

	public sealed class ApplicationAssembly : SuppliedSource<IEnumerable<Assembly>, Assembly>
	{
		public static ApplicationAssembly Default { get; } = new ApplicationAssembly();
		ApplicationAssembly() : base( ApplicationAssemblyLocator.Default.Get, ApplicationAssemblies.Default.GetEnumerable ) {}
	}
}