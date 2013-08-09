using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Prism.Modularity;

namespace DragonSpark.Application
{
    [Singleton( typeof(IModuleMonitor), Priority = Priority.Lowest )]
    public class ModuleMonitor : IModuleMonitor
    {
        readonly IModuleCatalog catalog;

        public event EventHandler Loaded = delegate {};
		
        readonly IList<ModuleInfo> items = new List<ModuleInfo>();

        readonly IList<IModule> modules = new List<IModule>();

        public ModuleMonitor( IModuleCatalog catalog )
        {
            this.catalog = catalog;
        }

        public bool Load()
        {
            var candidates = catalog.Modules.Where( x => ( x.State > ModuleState.NotStarted && x.State < ModuleState.Initialized ) || modules.Any( y => ModuleInfoExtensions.GetAssembly( x ) == y.GetType().Assembly ) ).ToArray();
            items.Clear();
            items.AddAll( candidates );
            var result = Check();
            return result;
        }

        public void Mark( IModule target )
        {
            modules.Add( target );

            Check();
        }

        bool Check()
        {
            var result = modules.Any() && items.Any() && modules.Count == items.Count;
            result.IsTrue( () => Threading.Application.Execute( () =>
            {
                var load = items.Select( x => x.GetAssembly() ).Select( x => modules.FirstOrDefault( y => y.GetType().Assembly == x ) ).ToArray();
                load.Apply( x => x.Load() );
                Loaded( this, EventArgs.Empty );
            } ) );
            return result;
        }
    }
}