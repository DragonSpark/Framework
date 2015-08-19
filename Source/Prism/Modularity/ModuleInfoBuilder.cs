// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;

namespace Prism.Modularity
{
    [Serializable]
    public class ModuleInfoBuilder : IModuleInfoBuilder
    {
        readonly IAttributeDataProvider provider;

        public ModuleInfoBuilder() : this( new AttributeDataProvider() )
        {}

        public ModuleInfoBuilder( IAttributeDataProvider provider )
        {
            this.provider = provider;
        }

        protected IAttributeDataProvider Provider
        {
            get { return provider; }
        }

        void Apply( ModuleInfo result, Type type )
        {
            var dependsOn = provider.GetAll<string>( typeof(ModuleDependencyAttribute), type, "ModuleName" );
            result.DependsOn.AddRange( dependsOn );
        }

        public ModuleInfo CreateModuleInfo(Type type)
        {
            string moduleName = provider.Get<string>( typeof(ModuleAttribute), type, "ModuleName" ) ?? type.Name;
            var result = Create( type, moduleName, type.AssemblyQualifiedName );
            Apply( result, type );
            return result;
        }

        protected virtual ModuleInfo Create( Type host, string moduleName, string assemblyQualifiedName )
        {
            var result = new ModuleInfo( moduleName, assemblyQualifiedName );
            return result;
        }
    }
}