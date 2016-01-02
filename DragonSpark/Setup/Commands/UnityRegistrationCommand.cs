using DragonSpark.ComponentModel;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;
using System;
using System.Windows.Markup;

namespace DragonSpark.Setup.Commands
{
	public abstract class UnityRegistrationCommand : UnityCommand, IRegistrationTypeContext
	{
		public string BuildName { get; set; }
		
		[Ambient]
		public Type RegistrationType  { get; set; }
		
		[Activate( typeof(ContainerControlledLifetimeManager) )]
		public LifetimeManager Lifetime { get; set; }
	}
}