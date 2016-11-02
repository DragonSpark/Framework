using DragonSpark.Extensions;
using DragonSpark.Specifications;
using System.Reflection;

namespace DragonSpark.Application
{
	public class RegisteredAssemblySpecification : SpecificationBase<Assembly>
	{
		public static RegisteredAssemblySpecification Default { get; } = new RegisteredAssemblySpecification();
		RegisteredAssemblySpecification() {}

		public override bool IsSatisfiedBy( Assembly parameter ) => parameter.Has<RegistrationAttribute>();
	}
}