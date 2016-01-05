using DragonSpark.Activation.IoC;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Parameters;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Windows.Testing.Setup
{
	public class UnitySetupTests : Tests
	{
		public UnitySetupTests( ITestOutputHelper output ) : base( output )
		{}

		[Theory, UnitySetup.AutoData]
		public void Extension( [Located]IUnityContainer sut )
		{
			Assert.NotNull( sut.Configure<TestExtension>() );
		}

		[Theory, UnitySetup.AutoData]
		public void RegisteredName( [Located]IUnityContainer sut )
		{
			Assert.NotNull( sut.Resolve<Singleton>( "SomeName" ) );
		}
	}

	public class FixtureExtension : UnityContainerExtension
	{
		[Value( typeof(SetupAutoDataContext) )]
		public AutoData Setup { get; set; }

		[BuildUp]
		protected override void Initialize()
		{
			Container.Registration<EnsuredRegistrationSupport>().Instance( Setup.Fixture );

			Container.Extend().Policies.Insert( 0, new FixtureBuildPlanStrategy() );
		}
	}

	public class FixtureBuildPlanStrategy : IBuildPlanPolicy
	{
		public void BuildUp( IBuilderContext context )
		{
			var fixture = context.NewBuildUp<IFixture>();
		}
	}
}