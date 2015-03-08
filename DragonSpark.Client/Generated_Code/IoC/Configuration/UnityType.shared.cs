using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "InjectionMembers" )]
	public class UnityType : IContainerConfigurationCommand
	{
		public string BuildName { get; set; }
		
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type RegistrationType  { get; set; }
		
		// [TypeConverter( typeof(TypeNameConverter) )]
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
			Microsoft.Practices.Unity.Utility.Guard.ArgumentNotNull(container, "container");

			var type = MapTo ?? RegistrationType;

			// InjectionMembers.Any().IsFalse( () => InjectionMembers.Add( new InjectionConstructor() ) );

			var members = InjectionMembers.Select( x => x.Create( container, type ) ).ToArray();
			container.RegisterType( RegistrationType, type, BuildName, Lifetime.Transform( item => item.Instance ), members );

			TypeConfigurations.Apply( item => item.Configure( container, this ) );
		}
	}
}