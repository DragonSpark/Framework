using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public sealed class ApplicationAssemblyLocator : ParameterizedSourceBase<IEnumerable<Assembly>, Assembly>
	{
		readonly Func<Assembly> defaultSource;

		public static ApplicationAssemblyLocator Default { get; } = new ApplicationAssemblyLocator();
		ApplicationAssemblyLocator() : this( AppDomain.CurrentDomain ) {}

		public ApplicationAssemblyLocator( AppDomain domain ) : this( new SuppliedSource<AppDomain, Assembly>( DomainApplicationAssemblies.Default.Get, domain ).Get ) {}

		public ApplicationAssemblyLocator( Func<Assembly> defaultSource )
		{
			this.defaultSource = defaultSource;
		}

		public override Assembly Get( IEnumerable<Assembly> parameter ) => Application.ApplicationAssemblyLocator.Default.Get( parameter ) ?? defaultSource();
	}
}