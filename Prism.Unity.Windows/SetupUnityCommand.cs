using Prism.Regions;

namespace Prism.Unity.Windows
{
    public class SetupUnityCommand : Unity.SetupUnityCommand
    {
        protected override void ConfigureContainer( SetupContext context )
        {
            base.ConfigureContainer( context );

            if (context.UseDefaultConfiguration)
            {
                context.RegisterTypeIfMissing(typeof(RegionAdapterMappings), typeof(RegionAdapterMappings), true);
                context.RegisterTypeIfMissing(typeof(IRegionManager), typeof(RegionManager), true);
                context.RegisterTypeIfMissing(typeof(IRegionViewRegistry), typeof(RegionViewRegistry), true);
                context.RegisterTypeIfMissing(typeof(IRegionBehaviorFactory), typeof(RegionBehaviorFactory), true);
                context.RegisterTypeIfMissing(typeof(IRegionNavigationJournalEntry), typeof(RegionNavigationJournalEntry), false);
                context.RegisterTypeIfMissing(typeof(IRegionNavigationJournal), typeof(RegionNavigationJournal), false);
                context.RegisterTypeIfMissing(typeof(IRegionNavigationService), typeof(RegionNavigationService), false);
                context.RegisterTypeIfMissing(typeof(IRegionNavigationContentLoader), typeof(UnityRegionNavigationContentLoader), true);
            }
        }
    }
}
