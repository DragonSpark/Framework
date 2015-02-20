﻿using System.Windows.Markup;
using Microsoft.Practices.Unity;

namespace DragonSpark.Server.Legacy
{
	[ContentProperty( "Configurators" )]
	public class ApplicationConfigurationCommand : Activation.IoC.Commands.ApplicationConfigurationCommand
	{
		protected override void OnConfigure( IUnityContainer container )
		{
			base.OnConfigure( container );
		}
	}
}
