using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	public abstract class RegionViewRegistrationAction
	{
		protected internal abstract void Process( IUnityContainer container, IRegion region, Type viewType );
	}
}