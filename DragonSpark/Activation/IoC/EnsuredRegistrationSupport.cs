using Microsoft.Practices.Unity;
using System.Reflection;

namespace DragonSpark.Activation.IoC
{
	public class EnsuredRegistrationSupport : RegistrationSupport
	{
		// public EnsuredRegistrationSupport( IUnityContainer container, IMessageLogger logger ) : base( container, logger, new Assembly[0], NotRegisteredSpecification.Instance ) {}

		public EnsuredRegistrationSupport( IUnityContainer container, Assembly[] applicationAssemblies ) : base( container, applicationAssemblies, NotRegisteredSpecification.Instance ) {}
	}
}