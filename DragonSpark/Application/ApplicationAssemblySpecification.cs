using DragonSpark.Extensions;
using DragonSpark.Specifications;
using System.Reflection;

namespace DragonSpark.Application
{
	public class ApplicationAssemblySpecification : SpecificationBase<Assembly>
	{
		public static ApplicationAssemblySpecification Default { get; } = new ApplicationAssemblySpecification();
		ApplicationAssemblySpecification() {}

		public override bool IsSatisfiedBy( Assembly parameter ) => parameter.Has<RegistrationAttribute>();
	}
}