using System;
using System.Diagnostics;
using DragonSpark.Activation.IoC;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public interface IServiceLocationAuthority
	{
		void Register( Type item, bool enabled );

		bool IsAllowed( Type type );
	}

	public class FixtureExtension : UnityContainerExtension
	{
		[Value( typeof( SetupAutoDataContext ) )]
		public AutoData Setup { get; set; }

		[BuildUp]
		protected override void Initialize()
		{
			this.BuildUp();

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