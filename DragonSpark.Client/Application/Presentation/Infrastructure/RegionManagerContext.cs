using System.Windows;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public static class RegionManagerContext
    {
        public static readonly DependencyProperty AssignProperty = DependencyProperty.RegisterAttached( "Assign", typeof(IRegionManager), typeof(RegionManagerContext), new PropertyMetadata( OnAssignPropertyChanged ) );

        static void OnAssignPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
        {
            ServiceLocation.With<IRegionManagerService>( x => x.Apply( o, e.NewValue.To<IRegionManager>() ) );
        }

        public static IRegionManager GetAssign( FrameworkElement element )
        {
            return (IRegionManager)element.GetValue( AssignProperty );
        }

        public static void SetAssign( FrameworkElement element, IRegionManager value )
        {
            element.SetValue( AssignProperty, value );
        }
    }
}