using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Modularity
{
	[Persistent]
	public class ModuleMonitor : IModuleMonitor
	{
		readonly IModuleCatalog catalog;
	
		readonly IList<ModuleInfo> loading = new List<ModuleInfo>();

		readonly TaskCompletionSource<bool> source = new TaskCompletionSource<bool>( false );

		readonly IList<IMonitoredModule> loaded = new List<IMonitoredModule>();

		public ModuleMonitor( IModuleCatalog catalog )
		{
			this.catalog = catalog;
		}

		public Task<bool> Load()
		{
			var adapter = typeof(IMonitoredModule).Adapt();
					
			var items = catalog.Modules
				.Where( x => adapter.IsAssignableFrom( x.ResolveType() ) )
				.Where( x => x.State > ModuleState.NotStarted && x.State < ModuleState.Initialized || loaded.Any( y => Equals( x.GetAssembly(), y.GetType().Assembly() ) ) )
				.ToArray();
			loading.Clear();
			loading.AddRange( items );
			Update();
			return source.Task;
		}

		public void MarkAsLoaded( IMonitoredModule target )
		{
			loaded.Add( target );

			Update();
		}

		void Update()
		{
			var complete = loaded.Any() == loading.Any() && loaded.Count == loading.Count;
			complete.IsTrue( OnComplete );
		}

		protected virtual void OnComplete()
		{
			var load = loading.Select( x => x.GetAssembly() ).Select( x => loaded.FirstOrDefault( y => Equals( y.GetType().Assembly(), x ) ) ).ToArray();
			load.Each( x => x.Load() );
			source.TrySetResult( true );
			loading.Clear();
			loaded.Clear();
		}
	}
}