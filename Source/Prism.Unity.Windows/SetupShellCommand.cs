using Microsoft.Practices.Unity;
using Prism.Logging;
using Prism.Regions;
using System.Windows;
using System.Windows.Markup;

namespace Prism.Unity.Windows
{
    [ContentProperty( "Shell" )]
    public class SetupShellCommand : SetupCommand
    {
        public DependencyObject Shell { get; set; }

        protected override void Execute( SetupContext context )
        {
            context.Logger.Log(Properties.Resources.CreatingShell, Category.Debug, Priority.Low);
            if (this.Shell != null)
            {
                context.Logger.Log(Properties.Resources.SettingTheRegionManager, Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(this.Shell, context.Container().Resolve<IRegionManager>());

                context.Logger.Log(Properties.Resources.UpdatingRegions, Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                context.Logger.Log(Properties.Resources.InitializingShell, Category.Debug, Priority.Low);
                this.InitializeShell( context );
            }
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        protected virtual void InitializeShell( SetupContext context )
        {}
    }
}