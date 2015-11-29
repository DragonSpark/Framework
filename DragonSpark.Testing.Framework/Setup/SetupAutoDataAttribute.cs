using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Activation.Build;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupAutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly ISetup setup;

		public SetupAutoDataAttribute() : this( new Fixture( DefaultEngineParts.Instance ) )
		{}

		public SetupAutoDataAttribute( IFixture fixture ) : this( fixture, typeof(DefaultSetup) )
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