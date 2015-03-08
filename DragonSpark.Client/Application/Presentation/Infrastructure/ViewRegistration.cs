using System;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public abstract class ViewRegistration : UnityContainerTypeConfiguration
	{
		protected sealed override void Configure( IUnityContainer container, UnityType type )
		{
			base.Configure( container, type );

			Process( container, type.MapTo );
		}

		protected abstract void Process( IUnityContainer container, Type viewType );
	}
}