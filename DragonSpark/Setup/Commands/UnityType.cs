using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Diagnostics;

namespace DragonSpark.Setup.Commands
{
	[ContentProperty( nameof(InjectionMembers) )]
	public class UnityType : UnityRegistrationCommand
	{
		public Type MapTo { get; set; }
		
		public System.Collections.ObjectModel.Collection<InjectionMember> InjectionMembers => injectionMembers.Value;
		readonly Lazy<System.Collections.ObjectModel.Collection<InjectionMember>> injectionMembers = new Lazy<System.Collections.ObjectModel.Collection<InjectionMember>>( () => new System.Collections.ObjectModel.Collection<InjectionMember>() );

		/*public System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration> TypeConfigurations => typeConfigurations.Value;
		readonly Lazy<System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration>> typeConfigurations = new Lazy<System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration>>( () => new System.Collections.ObjectModel.Collection<IUnityContainerTypeConfiguration>() );*/

		protected override void OnExecute( ISetupParameter parameter )
		{
			var type = MapTo ?? RegistrationType;

			InjectionMembers.Any().IsFalse( () => InjectionMembers.Add( new InjectionConstructor() ) );

			var mapping = string.Concat( RegistrationType.ToString(), RegistrationType != type ? $" -> {type}" : string.Empty );
			MessageLogger.Information( $"Registering Unity Type: {mapping}" );
			Container.RegisterType( RegistrationType, type, BuildName, Lifetime, InjectionMembers.ToArray() );

			/*TypeConfigurations.Each( item => item.Configure( container, this ) );*/
		}
	}
}