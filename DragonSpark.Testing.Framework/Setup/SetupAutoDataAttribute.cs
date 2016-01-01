using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Reflection;
using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing.Framework.Setup
{
	public class MoqAutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		public MoqAutoDataAttribute() : base( new Fixture( DefaultEngineParts.Instance ).Customize( new AutoMoqCustomization() ) )
		{}
	}

	public class FixtureFactory : FactoryBase<IFixture>
	{
		public static FixtureFactory Instance { get; } = new FixtureFactory();

		protected override IFixture CreateItem()
		{
			var result = new Fixture( SetupEngineParts.Instance );
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
			var argument = new SetupAutoDataParameter( Fixture, methodUnderTest );
			using ( var parameter = new SetupParameter( argument ) )
			{
				setup.Run( parameter );
				var result = base.GetData( methodUnderTest );
				return result;
			}
		}
	}

	public class SetupParameter : SetupParameter<SetupAutoDataParameter>
	{
		public SetupParameter( SetupAutoDataParameter arguments ) : base( arguments )
		{}
	}
}