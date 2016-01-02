using System;
using System.Collections.Generic;
using DragonSpark.Modularity;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	public class MockModuleTypeLoader : IModuleTypeLoader
	{
		public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged = delegate {};
		public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted = delegate {};
		public bool CanLoadModuleTypeReturnValue = true;
		public Exception LoadCompletedError;

		public bool CanLoadModuleType( ModuleInfo moduleInfo )
		{
			return CanLoadModuleTypeReturnValue;
		}

		public void LoadModuleType( ModuleInfo moduleInfo )
		{
			LoadedModules.Add( moduleInfo );
			RaiseLoadModuleCompleted( new LoadModuleCompletedEventArgs( moduleInfo, LoadCompletedError ) );
		}

		public List<ModuleInfo> LoadedModules { get; } = new List<ModuleInfo>();

		public void RaiseLoadModuleProgressChanged( ModuleDownloadProgressChangedEventArgs e )
		{
			ModuleDownloadProgressChanged( this, e );
		}

		public void RaiseLoadModuleCompleted( ModuleInfo moduleInfo, Exception error )
		{
			RaiseLoadModuleCompleted( new LoadModuleCompletedEventArgs( moduleInfo, error ) );
		}

		public void RaiseLoadModuleCompleted( LoadModuleCompletedEventArgs e )
		{
			LoadModuleCompleted( this, e );
		}
	}
}