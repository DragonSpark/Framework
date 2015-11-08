using DragonSpark.Extensions;
using DragonSpark.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Modularity
{
	/// <summary>
	/// Defines the contract for the modules deployed in the application.
	/// </summary>
	public interface IModule
	{
		void Initialize();

		void Load();
	}

	public class Module<TCommand> : Module where TCommand : ICommand
	{
		public Module( IModuleMonitor moduleMonitor, SetupContext context ) : base( moduleMonitor, context )
		{}

		protected override IEnumerable<ICommand> DetermineCommands()
		{
			var result = Activator.Create<TCommand>().Append().Cast<ICommand>().ToArray();
			return result;
		}
	}

	public class Module : IModule
	{
		readonly IModuleMonitor moduleMonitor;
		readonly SetupContext context;

		public Module( IModuleMonitor moduleMonitor, SetupContext context )
		{
			this.moduleMonitor = moduleMonitor;
			this.context = context;
		}

		void IModule.Initialize()
		{
			Initialize();

			moduleMonitor.MarkAsLoaded( this );
		}

		protected virtual void Initialize()
		{}

		void IModule.Load()
		{
			Load();
		}

		protected virtual void Load()
		{
			var commands = DetermineCommands();
			commands.Where( command => command.CanExecute( context ) ).Apply( x => x.Execute( context ) );
		}

		protected virtual IEnumerable<ICommand> DetermineCommands()
		{
			var result = GetType().GetTypeInfo().Assembly.ExportedTypes.Where( typeof(ICommand).IsAssignableFrom ).Select( Activator.CreateInstance<ICommand> ).ToArray();
			return result;
		}
	}

	public interface IModuleMonitor
	{
		Task<bool> Load();

		void MarkAsLoaded( IModule target );
	}

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

	public static class ModuleInfoExtensions
	{
		public static Assembly GetAssembly( this ModuleInfo target )
		{
			var result = Type.GetType( target.ModuleType, true )?.GetTypeInfo().Assembly;
			return result;
		}
	}
}