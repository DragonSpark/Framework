using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	public class RegisterViewTypeWithRegionAction : RegionViewRegistrationAction
	{
		protected internal override void Process( IUnityContainer container, IRegion region, Type viewType )
		{
			region.RegionManager.RegisterViewWithRegion( region.Name, viewType );
		}
	}
}