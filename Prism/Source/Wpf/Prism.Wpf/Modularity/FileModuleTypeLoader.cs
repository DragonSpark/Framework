// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.IO;

namespace Prism.Modularity
{
    /// <summary>
    /// Loads modules from an arbitrary location on the filesystem. This typeloader is only called if 
    /// <see cref="ModuleInfo"/> classes have a Ref parameter that starts with "file://". 
    /// This class is only used on the Desktop version of the Prism Library.
    /// </summary>
    public class FileModuleTypeLoader : LocalModuleTypeLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileModuleTypeLoader"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is disposed of in the Dispose method.")]
        public FileModuleTypeLoader()
            : base(new AssemblyResolver())
        {}

        protected override long DetermineSize( string path )
        {
            var result = File.Exists( path ) ? new FileInfo( path ).Length : -1L;
            return result;
        }
    }
}
