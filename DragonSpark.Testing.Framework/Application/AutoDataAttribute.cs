using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Testing.Framework.Application.Setup;
using JetBrains.Annotations;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public class AutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly static Func<IFixture> DefaultFixtureFactory = FixtureFactory<DefaultAutoDataCustomization>.Default.Get;
		
		public AutoDataAttribute() : this( DefaultFixtureFactory ) {}

		[UsedImplicitly]
		protected AutoDataAttribute( Func<IFixture> fixture ) : base( FixtureContext.Default.WithInstance( fixture() ) ) {}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			var application = ApplicationFactory.Default.Get( methodUnderTest );
			application.Run( new AutoData( Fixture, methodUnderTest ) );

			var result = base.GetData( methodUnderTest );
			return result;
		}
	}

	public class AutoConfiguredDataAttribute : AutoDataAttribute
	{
		readonly static Func<IFixture> Factory = FixtureFactory<AutoConfiguredDataCustomization>.Default.Get;
		public AutoConfiguredDataAttribute() : base( Factory ) {}
	}
}