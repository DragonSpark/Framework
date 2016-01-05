using DragonSpark.Activation;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Aspects;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;

namespace DragonSpark.Testing.Framework
{
	public class RegisterFactoryAttribute : RegistrationAttribute
	{
		readonly Type factoryType;

		public RegisterFactoryAttribute( [OfType( typeof(IFactory) )]Type factoryType ) : base( factoryType.Adapt().GetResultType() )
		{
			this.factoryType = factoryType;
		}

		protected override void Customize( IFixture fixture, IServiceRegistry registry )
		{
			var factory = fixture.Create<IFactory>( factoryType );
			var types = new[] { MappedTo, RegistrationType }.Distinct();
			types.Each( type => registry.RegisterFactory( type, factory ) );

			var temp = fixture.Create<object>( MappedTo );
		}
	}
}