using PostSharp.Patterns.Contracts;
using System;

namespace DragonSpark.Modularity
{
	public class ModuleTypeLoader : IModuleTypeLoader, IDisposable
	{
		/// <summary>
		/// Raised repeatedly to provide progress as modules are loaded in the background.
		/// </summary>
		public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged = delegate {};

		protected void RaiseModuleDownloadProgressChanged(ModuleInfo moduleInfo, long bytesReceived, long totalBytesToReceive)
		{
			RaiseModuleDownloadProgressChanged(new ModuleDownloadProgressChangedEventArgs(moduleInfo, bytesReceived, totalBytesToReceive));
		}

		protected void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs e)
		{
			ModuleDownloadProgressChanged(this, e);
		}

		/// <summary>
		/// Raised when a module is loaded or fails to load.
		/// </summary>
		public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted = delegate {};

		protected void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
		{
			this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
		}

		protected void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
		{
			LoadModuleCompleted(this, e);
		}

		/// <summary>
		/// Evaluates the <see cref="ModuleInfo.Ref"/> property to see if the current typeloader will be able to retrieve the <paramref name="moduleInfo"/>.
		/// Returns true if the <see cref="ModuleInfo.Ref"/> property starts with "file://", because this indicates that the file
		/// is a local file. 
		/// </summary>
		/// <param name="moduleInfo">Module that should have it's type loaded.</param>
		/// <returns>
		/// 	<see langword="true"/> if the current typeloader is able to retrieve the module, otherwise <see langword="false"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="moduleInfo"/> is null.</exception>
		public virtual bool CanLoadModuleType([Required]ModuleInfo moduleInfo)
		{
			return CanLoad( moduleInfo );
		}

		protected virtual bool CanLoad( ModuleInfo moduleInfo )
		{
			return true;
		}
		
		/// <summary>
		/// Retrieves the <paramref name="moduleInfo"/>.
		/// </summary>
		/// <param name="moduleInfo">Module that should have it's type loaded.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is rethrown as part of a completion event")]
		public virtual void LoadModuleType([Required]ModuleInfo moduleInfo)
		{
			try
			{
				Load( moduleInfo );
			}
			catch (Exception ex)
			{
				this.RaiseLoadModuleCompleted(moduleInfo, ex);
			}
		}

		protected virtual void Load( ModuleInfo moduleInfo )
		{
			RaiseLoadModuleCompleted( moduleInfo, null );
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the associated <see cref="AssemblyResolver"/>.
		/// </summary>
		/// <param name="disposing">When <see langword="true"/>, it is being called from the Dispose method.</param>
		protected virtual void Dispose(bool disposing)
		{}

		#endregion
	}
}
