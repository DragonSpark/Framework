using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class FixtureFactory : FactoryBase<IFixture>
	{
		public static FixtureFactory Instance { get; } = new FixtureFactory();

		protected override IFixture CreateItem()
		{
			return new Fixture( DefaultEngineParts.Instance );
		}
	}

	public class SetupAutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly ISetup setup;

		public SetupAutoDataAttribute() : this( FixtureFactory.Instance.Create() )
		{}

		public SetupAutoDataAttribute( IFixture fixture ) : this( fixture, typeof(DefaultSetup) )
		{}

		public SetupAutoDataAttribute( Type setupType ) : this( FixtureFactory.Instance.Create(), setupType )
		{}

		public SetupAutoDataAttribute( IFixture fixture, Type setupType ) : this( fixture, ActivateFactory<ISetup>.Instance.Locked( factory => factory.CreateUsing( setupType ) ) )
		{}

		public SetupAutoDataAttribute( IFixture fixture, ISetup setup ) : base( fixture )
		{
			this.setup = setup;
		}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			setup.Run( new SetupAutoDataContext( Fixture, methodUnderTest ) );

			var result = base.GetData( methodUnderTest );
			return result;
		}
	}
}