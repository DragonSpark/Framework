using System;
using System.Windows.Markup;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public abstract class RegisterWithRegionActionBase : RegionViewRegistrationAction
	{
		protected void Register( IRegion region, Type viewType, object item )
		{
			item.As<IRegionContract>( x =>
			{
				x.ContractName = x.ContractName ?? viewType.FullName;
				x.TargetName = x.TargetName ?? region.Name;
			} );
			region.Add( item );
		}
	}

	[ContentProperty( "Instance" )]
	public class RegisterInstanceWithRegionAction : RegisterWithRegionActionBase
	{
		public object Instance { get; set; }

		protected internal override void Process( IUnityContainer container, IRegion region, Type viewType )
		{
			Register( region, viewType, Instance );
		}
	}
}