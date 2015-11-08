using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using System;

namespace DragonSpark.Testing.Framework
{
	/*public class FactoryAttribute : CustomizeAttribute
	{
		readonly Type factoryType;

		public FactoryAttribute( Type factoryType )
		{
			this.factoryType = factoryType;
		}

		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var result = (ICustomization)Dynamic.InvokeConstructor( factoryType, parameter.ParameterType );
			return result;
		}
	}*/

	public class ContainerExtensionFactory : RegisterFactoryAttribute
	{
		public ContainerExtensionFactory( Type registrationType ) : base( registrationType )
		{}

		protected override Func<object> GetFactory( IFixture fixture, IServiceRegistry registry )
		{
			Func<object> result = () => fixture.Create<IUnityContainer>().EnsureExtension( MappedTo );
			return result;
		}
	}


	public abstract class RegisterFactoryAttribute : RegistrationAttribute
	{
		protected RegisterFactoryAttribute( Type registrationType ) : base( registrationType )
		{}

		/*protected RegisterFactoryAttribute( Type registrationType, Type mappedTo ) : base( registrationType, mappedTo )
		{}*/

		protected override void Customize( IFixture fixture, IServiceRegistry registry )
		{
			var factory = GetFactory( fixture, registry );
			registry.RegisterFactory( MappedTo, factory );
		}

		protected abstract Func<object> GetFactory( IFixture fixture, IServiceRegistry registry );
	}

	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true )]
	public abstract class RegistrationAttribute : Attribute, ICustomization
	{
		protected RegistrationAttribute( Type registrationType ) : this( registrationType, registrationType )
		{}

		protected RegistrationAttribute( Type registrationType, Type mappedTo )
		{
			RegistrationType = registrationType;
			MappedTo = mappedTo;
		}

		public Type RegistrationType { get; }

		public Type MappedTo { get; }

		void ICustomization.Customize( IFixture fixture )
		{
			Services.With<IServiceRegistry>( registry => Customize( fixture, registry ) );
		}

		protected abstract void Customize( IFixture fixture, IServiceRegistry registry );
	}
}