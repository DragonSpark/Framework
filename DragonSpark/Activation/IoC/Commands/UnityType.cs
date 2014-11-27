using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.Activation.IoC.Commands
{
	// [ContentProperty( "InjectionMembers" )]
	public class UnityType : IContainerConfigurationCommand
	{
		public string BuildName { get; set; }
		
		public Type RegistrationType  { get; set; }
		
		public Type MapTo { get; set; }
		
		public LifetimeManager Lifetime { get; set; }

		public ObservableCollection<InjectionMember> InjectionMembers
		{
			get { return injectionMembers ?? ( injectionMembers = new ObservableCollection<InjectionMember>() ); }
		}	ObservableCollection<InjectionMember> injectionMembers;

		public ObservableCollection<IUnityContainerTypeConfiguration> TypeConfigurations
		{
			get { return typeConfigurations ?? ( typeConfigurations = new ObservableCollection<IUnityContainerTypeConfiguration>() ); }
		}	ObservableCollection<IUnityContainerTypeConfiguration> typeConfigurations;
		
		public virtual void Configure( IUnityContainer container )
		{
			Guard.ArgumentNotNull( container, "container" );

			var type = MapTo ?? RegistrationType;

			// InjectionMembers.Any().IsFalse( () => InjectionMembers.Add( new InjectionConstructor() ) );

			var members = InjectionMembers.ToArray();
			container.RegisterType( RegistrationType, type, BuildName, Lifetime, members );

			TypeConfigurations.Apply( item => item.Configure( container, this ) );
		}
	}
}