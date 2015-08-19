

using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace Prism.Mef.Modularity
{
    /// <summary>
    /// Loads modules from an arbitrary location on the filesystem. This typeloader is only called if 
    /// <see cref="ModuleInfo"/> classes have a Ref parameter that starts with "file://". 
    /// This class is only used on the Desktop version of the Prism Library when used with Managed Extensibility Framework.
    /// </summary>  
    [Export]
    public class MefFileModuleTypeLoader : LocalModuleTypeLoader
    {
        private const string RefFilePrefix = "file://";
        private readonly HashSet<Uri> downloadedUris = new HashSet<Uri>();

        // disable the warning that the field is never assigned to, and will always have its default value null
        // as it is imported by MEF
#pragma warning disable 0649
        [Import(AllowRecomposition = false)]
        private AggregateCatalog aggregateCatalog;
#pragma warning restore 0649

        /// <summary>
        /// Initializes a new instance of the MefFileModuleTypeLoader class.
        /// This instance is used to load requested module types.
        /// </summary>
        public MefFileModuleTypeLoader()
        {
        }

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

                this.aggregateCatalog.Catalogs.Add(new AssemblyCatalog(path));

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
    }
}