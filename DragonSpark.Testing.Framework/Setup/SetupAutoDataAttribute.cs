using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
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
			var result = new Fixture( DefaultEngineParts.Instance );
			return result;
		}
	}

	public class SetupAutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly Type setupType;

		public SetupAutoDataAttribute( Type setupType ) : this( FixtureFactory.Instance.Create(), setupType )
		{}

		public SetupAutoDataAttribute( IFixture fixture, [OfType( typeof(ISetup) )]Type setupType ) : base( fixture )
		{
			this.setupType = setupType;
		}

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			var setup = ActivateFactory<ISetup>.Instance.CreateUsing( setupType );
			setup.Run( new SetupAutoDataParameter( Fixture, methodUnderTest ) );
			// setup.Commands.Each( BuildPropertyRepository.Reset );

			var result = base.GetData( methodUnderTest );
			return result;
		}
	}
}