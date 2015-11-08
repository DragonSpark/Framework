using System;
using System.Collections.Generic;
using System.IO;
using DragonSpark.Modularity;
using DragonSpark.Setup;

namespace DragonSpark.Windows.Modularity
{
	/// <summary>
	/// Loads modules from an arbitrary location on the filesystem. This typeloader is only called if 
	/// <see cref="ModuleInfo"/> classes have a Ref parameter that starts with "file://". 
	/// This class is only used on the Desktop version of the Prism Library.
	/// </summary>
	[RegisterAs( typeof(IModuleTypeLoader) )]
	public class FileModuleTypeLoader : LocalModuleTypeLoader
	{
		private const string RefFilePrefix = "file://";
		private readonly HashSet<Uri> downloadedUris = new HashSet<Uri>();

		/// <summary>
		/// Initializes a new instance of the <see cref="FileModuleTypeLoader"/> class.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is disposed of in the Dispose method.")]
		public FileModuleTypeLoader()
			: this(new AssemblyResolver())
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalModuleTypeLoader"/> class.
		/// </summary>
		/// <param name="assemblyResolver">The assembly resolver.</param>
		public FileModuleTypeLoader(IAssemblyResolver assemblyResolver)
		{
			AssemblyResolver = assemblyResolver;
		}

		protected IAssemblyResolver AssemblyResolver { get; }

		protected override bool CanLoad( ModuleInfo moduleInfo )
		{
			var dynamicModuleInfo = moduleInfo as DynamicModuleInfo;
			var result = dynamicModuleInfo == null || ( dynamicModuleInfo.Ref != null && dynamicModuleInfo.Ref.StartsWith(RefFilePrefix, StringComparison.Ordinal) );
			return result;
		}

		protected override void Load( ModuleInfo moduleInfo )
		{
			var dynamicModuleInfo = moduleInfo as DynamicModuleInfo;
			
			Uri uri = dynamicModuleInfo != null ? new Uri(dynamicModuleInfo.Ref, UriKind.RelativeOrAbsolute) : null;

			// If this module has already been downloaded, I fire the completed event.
			if (uri == null || this.IsSuccessfullyDownloaded(uri))
			{
				base.Load( moduleInfo );
			}
			else
			{
				var @ref = dynamicModuleInfo.Ref;
				var offset = @ref.StartsWith(RefFilePrefix + "/", StringComparison.Ordinal ) ? 1 : 0;
				var path = @ref.Substring(RefFilePrefix.Length + offset);

				long fileSize = DetermineSize( path );

				// Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
				this.RaiseModuleDownloadProgressChanged(moduleInfo, 0, fileSize);

				AssemblyResolver.LoadAssemblyFrom(@ref);

				// Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
				this.RaiseModuleDownloadProgressChanged(moduleInfo, fileSize, fileSize);

				// I remember the downloaded URI.
				this.RecordDownloadSuccess(uri);

				this.RaiseLoadModuleCompleted(moduleInfo, null);
			}
		  
		}

		protected virtual long DetermineSize( string path )
		{
			var result = File.Exists( path ) ? new FileInfo( path ).Length : -1L;
			return result;
		}

		private bool IsSuccessfullyDownloaded(Uri uri)
		{
			lock (this.downloadedUris)
			{
				return this.downloadedUris.Contains(uri);
			}
		}

		private void RecordDownloadSuccess(Uri uri)
		{
			lock (this.downloadedUris)
			{
				this.downloadedUris.Add(uri);
			}
		}

		protected override void Dispose( bool disposing )
		{
			var disposableResolver = this.AssemblyResolver as IDisposable;
			disposableResolver?.Dispose();
		}
	}
}
