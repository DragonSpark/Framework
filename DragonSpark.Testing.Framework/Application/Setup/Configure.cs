using DragonSpark.Activation;
using DragonSpark.Application.Setup;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework.Runtime;
using System.Composition;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class Configure : AlterationBase<IActivator>
	{
		[Export( typeof(IAlteration<IActivator>) )]
		public static Configure Default { get; } = new Configure();
		Configure() {}

		public override IActivator Get( IActivator parameter ) => 
			new CompositeActivator( new InstanceRepository( FixtureContext.Default, MethodContext.Default ), new FixtureServiceProvider( FixtureContext.Default.Get() ), parameter );
	}
}