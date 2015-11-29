using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Extensions;

namespace DragonSpark.Modularity
{
	public class ModuleMonitor : IModuleMonitor
	{
		readonly IModuleCatalog catalog;
	
		readonly IList<ModuleInfo> loading = new List<ModuleInfo>();

		readonly IList<IModule> loaded = new List<IModule>();

		readonly TaskCompletionSource<bool> source = new TaskCompletionSource<bool>( false );

		public ModuleMonitor( IModuleCatalog catalog )
		{
			this.catalog = catalog;
		}

		public Task<bool> Load()
		{
			var items = catalog.Modules.Where( x => ( x.State > ModuleState.NotStarted && x.State < ModuleState.Initialized ) || loaded.Any( y => Equals( x.GetAssembly(), y.GetType().Assembly() ) ) ).ToArray();
			loading.Clear();
			loading.AddRange( items );
			Update();
			return source.Task;
		}

		public void MarkAsLoaded( IModule target )
		{
			loaded.Add( target );

			Update();
		}

		void Update()
		{
			var complete = loaded.Any() && loading.Any() && loaded.Count == loading.Count;
			complete.IsTrue( OnComplete );
		}

		protected virtual void OnComplete()
		{
			var load = loading.Select( x => x.GetAssembly() ).Select( x => loaded.FirstOrDefault( y => Equals( y.GetType().Assembly(), x ) ) ).ToArray();
			load.Apply( x => x.Load() );

			source.TrySetResult( true );
		}
	}
}