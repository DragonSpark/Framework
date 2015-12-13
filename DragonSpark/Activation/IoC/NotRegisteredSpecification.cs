using DragonSpark.Runtime.Specifications;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	public class NotRegisteredSpecification : SpecificationBase<RegistrationSpecificationParameter>
	{
		public static NotRegisteredSpecification Instance { get; } = new NotRegisteredSpecification();

		protected override bool IsSatisfiedByContext( RegistrationSpecificationParameter context )
		{
			var result = base.IsSatisfiedByContext( context) && !context.Container.IsRegistered( context.Type );
			return result;
		}
	}
}