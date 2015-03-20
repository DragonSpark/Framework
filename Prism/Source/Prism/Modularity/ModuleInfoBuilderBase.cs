// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;

namespace Prism.Modularity
{
	public abstract class ModuleInfoBuilderBase : IModuleInfoBuilder
    {
        public ModuleInfo CreateModuleInfo(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var moduleAttribute = typeInfo.GetCustomAttribute<ModuleAttribute>();
            string moduleName = ( moduleAttribute != null ? moduleAttribute.ModuleName : null ) ?? type.Name;
            ModuleInfo moduleInfo = new ModuleInfo(moduleName, type.AssemblyQualifiedName)
            {
                InitializationMode =
                    moduleAttribute != null && moduleAttribute.OnDemand
                        ? InitializationMode.OnDemand
                        : InitializationMode.WhenAvailable,
                Ref = DetermineRef( typeInfo ),
            };
            var dependsOn = typeInfo.GetCustomAttributes<ModuleDependencyAttribute>().Select( attribute => attribute.ModuleName );
            moduleInfo.DependsOn.AddRange(dependsOn);
            return moduleInfo;
        }

        protected abstract string DetermineRef( TypeInfo type );
    }
}