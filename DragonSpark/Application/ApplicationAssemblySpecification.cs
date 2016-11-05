using DragonSpark.Specifications;
using System.Reflection;

namespace DragonSpark.Application
{
	public sealed class ApplicationAssemblySpecification : ConfigurableSpecification<Assembly>
	{
		public static ApplicationAssemblySpecification Default { get; } = new ApplicationAssemblySpecification();
		ApplicationAssemblySpecification() : base( assembly => assembly.IsDefined( typeof(ApplicationAttribute) ) ) {}
	}
}