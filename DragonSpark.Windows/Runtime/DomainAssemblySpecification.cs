using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public sealed class DomainAssemblySpecification : EqualitySpecification<Assembly>
	{
		public static DomainAssemblySpecification Default { get; } = new DomainAssemblySpecification();
		DomainAssemblySpecification() : this( DomainAssemblies.Default.Get( AppDomain.CurrentDomain ) ) {}

		public DomainAssemblySpecification( Assembly assembly ) : base( assembly ) {}
	}
}