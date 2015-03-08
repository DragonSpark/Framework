using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public interface IRegionManagerService
    {
        event EventHandler Applied;

        void Apply( DependencyObject target, IRegionManager regionManager );

        IEnumerable<IRegionManager> Current { get; }
    }
}