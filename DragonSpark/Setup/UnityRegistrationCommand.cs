using System;
using System.Windows.Markup;
using DragonSpark.ComponentModel;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
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