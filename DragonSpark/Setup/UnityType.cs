using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup
{
	public abstract class UnityCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			Configure( context.Container() );
		}

		protected abstract void Configure( IUnityContainer container );
	}

	public abstract class UnityRegistrationCommand : UnityCommand, IRegistrationTypeContext
	{
		public string BuildName { get; set; }
		
		[Ambient]
		public Type RegistrationType  { get; set; }
		
		[Activate( typeof(ContainerControlledLifetimeManager) )]
		public LifetimeManager Lifetime { get; set; }
	}

	public interface IRegistrationTypeContext
	{
		Type RegistrationType { get; }
	}

	[ContentProperty( "InjectionMembers" )]
	public class UnityType : UnityRegistrationCommand
	{
		public Type MapTo { get; set; }
		
		public System.Collections.ObjectModel.Collection<InjectionMember> InjectionMembers => injectionMembers.Value;
		readonly Lazy<System.Collections.ObjectModel.Collection<InjectionMember>> injectionMembers = new Lazy<System.Collections.ObjectModel.Collection<InjectionMember>>( () => new System.Collections.ObjectModel.Collection<InjectionMember>() );

		public System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration> TypeConfigurations => typeConfigurations.Value;
		readonly Lazy<System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration>> typeConfigurations = new Lazy<System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration>>( () => new System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration>() );

		protected override void Configure( IUnityContainer container )
		{
			var type = MapTo ?? RegistrationType;

			InjectionMembers.Any().IsFalse( () => InjectionMembers.Add( new InjectionConstructor() ) );

			var members = InjectionMembers.ToArray();
			container.RegisterType( RegistrationType, type, BuildName, Lifetime, members );

			TypeConfigurations.Apply( item => item.Configure( container, this ) );
		}
	}

	public abstract class InjectionMemberFactory<TMember> : FactoryBase<InjectionMemberContext, TMember> where TMember : InjectionMember
	{}

	public class InjectionMemberContext
	{
		public InjectionMemberContext( IUnityContainer container, Type targetType )
		{
			Container = container;
			TargetType = targetType;
		}

		public IUnityContainer Container { get; }

		public Type TargetType { get; }
	}

	[LifetimeManager( typeof(ContainerControlledLifetimeManager) )]
	public class AllTypesOfFactory : FactoryBase<Type, Array>
	{
		readonly IAssemblyProvider provider;

		public AllTypesOfFactory( IAssemblyProvider provider )
		{
			this.provider = provider;
		}

		protected override Array CreateFrom( Type resultType, Type parameter )
		{
			var type = parameter ?? resultType.GetInnerType() ?? resultType;
			var items = provider.GetAssemblies().SelectMany( assembly => assembly.ExportedTypes.Where( t => t.CanActivate( type ) ) ).Select( Activator.CreateInstance<object> ).ToArray();
			var result = Array.CreateInstance( type, items.Length );
			Array.Copy( items, result, items.Length );
			return result;
		}
	}


	public class InjectionFactoryFactory : InjectionMemberFactory<InjectionFactory>
	{
		readonly IFactory factory;
		readonly object parameter;

		public InjectionFactoryFactory( IFactory factory, object parameter )
		{
			this.factory = factory;
			this.parameter = parameter;
		}

		protected override InjectionFactory CreateFrom( Type resultType, InjectionMemberContext context )
		{
			var previous = context.Container.Registrations.FirstOrDefault( x => x.RegisteredType == context.TargetType && x.MappedToType != x.RegisteredType ).Transform( x => x.MappedToType );

			var result = new InjectionFactory( ( unityContainer, type, buildName ) =>
			{
				var item = Create( unityContainer, type, buildName ) ?? previous.Transform( x => context.Container.Resolve( x ) );
				return item;
			} );
			return result;
		}

		protected virtual object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = factory.Create( type, parameter );
			return result;
		}

		/*object IFactory.Create( Type resultType, object source )
		{
			var container = source.As<IUnityContainer>() ?? source.As<IBuilderContext>().Transform( x => x.NewBuildUp<IUnityContainer>() ) ?? Services.Locate<IUnityContainer>();
			var result = this.WithDefaults().Create( container, resultType, string.Empty );
			return result;
		}*/
	}

	// [ContentProperty( "BuildKey" )]
	/*public class InjectionFactory : FactoryBase
	{
		public NamedTypeBuildKey BuildKey { get; set; }

		public object Parameter { get; set; }

		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var factory = Activator.CreateNamedInstance<IFactory>( BuildKey.Type, BuildKey.Name ); // BuildKey.Transform( x => NamedTypeBuildKeyExtensions.Create<IFactory>( x, container ) );

			// var parameter = Parameter.AsTo<NamedTypeBuildKey, object>( x => x.Create( container ) ) ?? ( Parameter is NamedTypeBuildKey ? null : Parameter );

			var result = factory.Transform( x => x.Create( type, Parameter ) );
			return result;
		}
	}*/
}