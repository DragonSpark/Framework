using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using DragonSpark.Modularity;
using DragonSpark.Windows.Properties;

namespace DragonSpark.Windows.Modularity
{
    class RemotingModuleInfoProvider<TProvider> : IModuleInfoProvider where TProvider : MarshalByRefObject, IModuleInfoProvider
    {
        readonly AppDomain domain;
        readonly IModuleInfoProvider provider;

        public RemotingModuleInfoProvider( AppDomain domain, IModuleInfoBuilder builder, IEnumerable<string> assemblies, string directoryPath )
        {
            this.domain = domain;
            if (string.IsNullOrEmpty(directoryPath))
                throw new InvalidOperationException(Resources.ModulePathCannotBeNullOrEmpty);

            if (!Directory.Exists(directoryPath))
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.DirectoryNotFound, directoryPath));

            Type loaderType = typeof(TProvider);
            var arguments = new object[] { builder, assemblies, directoryPath };
            provider = (IModuleInfoProvider)domain.CreateInstanceFromAndUnwrap( loaderType.Assembly.Location, loaderType.FullName, false
                , BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance
                , null, arguments, null, null);
        }

        public IEnumerable<ModuleInfo> GetModuleInfos()
        {
            var result = provider.GetModuleInfos();
            return result;
        }
            
        /// <summary>
        /// Disposes the associated <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, disposes the associated <see cref="TextWriter"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                provider.Dispose();
                AppDomain.Unload(domain);
            }
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}