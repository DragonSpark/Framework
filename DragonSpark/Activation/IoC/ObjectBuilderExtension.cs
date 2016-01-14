using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace DragonSpark.Activation.IoC
{
	public class ObjectBuilderExtension : UnityContainerExtension
	{
		protected override void Initialize() => Context.Strategies.AddNew<ObjectBuilderStrategy>( UnityBuildStage.Initialization );

		public void Enable( bool on ) => Context.Policies.SetDefault<IObjectBuilderPolicy>( new ObjectBuilderPolicy( @on ) );
	}
}