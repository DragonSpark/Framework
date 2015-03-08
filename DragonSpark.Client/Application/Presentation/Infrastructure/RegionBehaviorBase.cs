using System;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	public abstract class RegionBehaviorBase<TControl> : RegionBehavior, IHostAwareRegionBehavior where TControl : DependencyObject
	{
		protected virtual TControl AssociatedControl { get; set; }

		protected override void OnAttach()
		{
			if ( AssociatedControl == null )
			{
				throw new InvalidOperationException();
			}
		}

		DependencyObject IHostAwareRegionBehavior.HostControl
		{
			get { return AssociatedControl; }
			set
			{
				var newValue = value as TControl;
				if ( newValue == null )
				{
					throw new InvalidOperationException();
				}

				if ( IsAttached )
				{
					throw new InvalidOperationException();
				}

				AssociatedControl = newValue;
			}
		}
	}
}