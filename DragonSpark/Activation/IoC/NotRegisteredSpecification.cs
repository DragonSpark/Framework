using DragonSpark.Runtime.Specifications;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	public class NotRegisteredSpecification : SpecificationBase<RegistrationSpecificationParameter>
	{
		public static NotRegisteredSpecification Instance { get; } = new NotRegisteredSpecification();

		protected override bool IsSatisfiedByParameter( RegistrationSpecificationParameter parameter )
		{
			var result = base.IsSatisfiedByParameter( parameter) && !parameter.Container.IsRegistered( parameter.Type );
			return result;
		}
	}
}