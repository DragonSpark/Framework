using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup.Registration
{
	public class RegisterFactoryForResultAttribute : RegistrationBaseAttribute
	{
		protected override void PerformRegistration( IServiceRegistry registry, Type subject )
		{
			var typeExtension = FactoryReflectionSupport.Instance.GetResultType( subject );
			typeExtension.With( type =>
			{
				registry.RegisterFactory( type, () =>
				{
					var factory = Activator.Current.Activate<FactoryBuiltObjectFactory>();
					var result = factory.Create( new ObjectFactoryParameter( subject ) );
					return result;
				} );
			} );
		}
	}
}