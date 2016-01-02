using System.Reflection;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	public class EnsuredRegistrationSupport : RegistrationSupport
	{
		public EnsuredRegistrationSupport( IUnityContainer container, Assembly[] applicationAssemblies ) : base( container, NotRegisteredSpecification.Instance, applicationAssemblies )
		{}
	}
}