using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup.Registration
{
	public class FactoryResultTypeRegistration : IConventionRegistration
	{
		public static FactoryResultTypeRegistration Instance { get; } = new FactoryResultTypeRegistration();

		FactoryResultTypeRegistration()
		{}

		public void Register( IServiceRegistry registry, Type subject )
		{
			var resultType = FactoryReflectionSupport.Instance.GetResultType( subject );
			resultType.With( type =>
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