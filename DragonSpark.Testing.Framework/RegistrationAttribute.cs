using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Windows.Runtime;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using System;
using UnityConventionRegistrationService = DragonSpark.Windows.Setup.UnityConventionRegistrationService;

namespace DragonSpark.Testing.Framework
{
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

		protected Type RegistrationType { get; }

		protected Type MappedTo { get; }

		void ICustomization.Customize( IFixture fixture )
		{
			var registry = DetermineRegistry( fixture );
			Customize( fixture, registry );
		}

		protected virtual IServiceRegistry DetermineRegistry( IFixture fixture )
		{
			var result = fixture.Create<IServiceRegistry>();
			return result;
		}

		protected abstract void Customize( IFixture fixture, IServiceRegistry registry );
	}

	public class RegisterFromConventionAttribute : ICustomization
	{
		readonly IConventionRegistrationProfileProvider provider;
		readonly UnityConventionRegistrationServiceFactory factory;
		readonly IConventionRegistrationService registrationService;

		public RegisterFromConventionAttribute() : this( new ConventionRegistrationProfileProvider( FilteredAssemblyProvider.Instance ), UnityConventionRegistrationServiceFactory.Instance )
		{}

		public RegisterFromConventionAttribute( IConventionRegistrationProfileProvider provider, UnityConventionRegistrationServiceFactory factory )
		{
			this.provider = provider;
			this.factory = factory;
		}

		public void Customize( IFixture fixture )
		{
			var service = factory.Create( fixture );
			var profile = provider.Retrieve();
			service.Register( profile );
		}
	}

	public class UnityConventionRegistrationServiceFactory : FactoryBase<IFixture, IConventionRegistrationService>
	{
		public static UnityConventionRegistrationServiceFactory Instance { get; } = new UnityConventionRegistrationServiceFactory();

		protected override IConventionRegistrationService CreateFrom( Type resultType, IFixture parameter )
		{
			return parameter.GetLocator().Transform( locator =>
			{
				return locator.Transform( x => x.GetInstance<IUnityContainer>(), parameter.TryCreate<IUnityContainer> ).Transform( container =>
				{
					var logger = parameter.GetLogger() ?? container.Resolve<ILogger>() ?? DebugLogger.Instance;
					var activator = locator.GetInstance<IActivator>() ?? parameter.TryCreate<IActivator>() ?? new Activation.IoC.Activator( container );
					var result = new UnityConventionRegistrationService( container, activator, logger );
					return result;
				});
			} );
		}
	}
}