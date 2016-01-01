using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Windows.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Modularity
{
	/// <summary>
	/// Handles AppDomain's AssemblyResolve event to be able to load assemblies dynamically in 
	/// the LoadFrom context, but be able to reference the type from assemblies loaded in the Load context.
	/// </summary>
	public class AssemblyResolver : IAssemblyResolver, IDisposable
    {
	    public static AssemblyResolver Instance { get; } = new AssemblyResolver();

	    readonly private List<AssemblyInfo> registeredAssemblies = new List<AssemblyInfo>();

        private bool handlesAssemblyResolve;

        /// <summary>
        /// Registers the specified assembly and resolves the types in it when the AppDomain requests for it.
        /// </summary>
        /// <param name="assemblyFilePath">The path to the assemly to load in the LoadFrom context.</param>
        /// <remarks>This method does not load the assembly immediately, but lazily until someone requests a <see cref="Type"/>
        /// declared in the assembly.</remarks>
        public void LoadAssemblyFrom(string assemblyFilePath)
        {
            if (!this.handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_AssemblyResolve;
                this.handlesAssemblyResolve = true;
            }

            Uri assemblyUri = GetFileUri(assemblyFilePath);

            if (assemblyUri == null)
            {
                throw new ArgumentException(Resources.InvalidArgumentAssemblyUri, nameof( assemblyFilePath ));
            }

            if (!File.Exists(assemblyUri.LocalPath))
            {
                throw new FileNotFoundException();
            }

            AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyUri.LocalPath);
            registeredAssemblies.Any(a => assemblyName == a.AssemblyName).IsFalse( () => new AssemblyInfo { AssemblyName = assemblyName, AssemblyUri = assemblyUri }.With( registeredAssemblies.Add ) );
        }

        private static Uri GetFileUri(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                return null;
            }

            Uri uri;
            if (!Uri.TryCreate(filePath, UriKind.Absolute, out uri))
            {
                return null;
            }

            return uri;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFrom")]
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            var result = this.registeredAssemblies.FirstOrDefault(a => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName) ).With( info => info.Assembly ?? ( info.Assembly = Assembly.LoadFrom( info.AssemblyUri.LocalPath ) ) );
			return result;
        }

        private class AssemblyInfo
        {
            public AssemblyName AssemblyName { get; set; }

            public Uri AssemblyUri { get; set; }

            public Assembly Assembly { get; set; }
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
        {
            if (this.handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve -= this.CurrentDomain_AssemblyResolve;
                this.handlesAssemblyResolve = false;
            }
        }

        #endregion
    }
}
