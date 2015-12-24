using DragonSpark.Activation;
using DragonSpark.Modularity;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using Microsoft.Practices.Unity;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Setup
{
	public class ProgramSetupTests : Tests
	{
		public ProgramSetupTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, Test, SetupAutoData( typeof(ProgramSetup) )]
		public void Extension( [Located]ModuleManager sut )
		{

		}
	}

	public class Module : DragonSpark.Modularity.Module
	{
		public Module( IActivator activator, IModuleMonitor moduleMonitor, SetupContext context ) : base( activator, moduleMonitor, context )
		{}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void Load()
		{
			base.Load();
		}
	}
}