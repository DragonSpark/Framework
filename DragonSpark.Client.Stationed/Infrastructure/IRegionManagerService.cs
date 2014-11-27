using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Client.Stationed.Infrastructure
{
	public interface IRegionManagerService
	{
		event EventHandler Applied;

		void Apply( DependencyObject target, IRegionManager regionManager );

		IEnumerable<IRegionManager> Current { get; }
	}
}