using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace DragonSpark.Activation.IoC
{
	public class ObjectBuilderExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			Context.Strategies.AddNew<ObjectBuilderStrategy>( UnityBuildStage.Initialization );

			var types = new[] { typeof(IAttributeProvider) };
			var policy = new ObjectBuilderPolicy( false );
			types.Each( type => Context.Policies.Set<IObjectBuilderPolicy>( policy, type ) );

			Container.Registration<EnsuredRegistrationSupport>().With( register =>
			{
				register.Instance<IObjectBuilder>( ObjectBuilder.Instance );
			});
		}

		public void Enable( bool on )
		{
			Context.Policies.SetDefault<IObjectBuilderPolicy>( new ObjectBuilderPolicy( on ) );
		}
	}
}