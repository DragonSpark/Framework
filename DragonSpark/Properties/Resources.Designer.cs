﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DragonSpark.Properties {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DragonSpark.Properties.Resources", typeof(Resources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not resolve type &quot;{0}&quot; with build name &quot;{1}&quot;..
        /// </summary>
        public static string Activator_CouldNotActivate {
            get {
                return ResourceManager.GetString("Activator_CouldNotActivate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;None&gt;.
        /// </summary>
        public static string Activator_None {
            get {
                return ResourceManager.GetString("Activator_None", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding UnityBootstrapperExtension to container..
        /// </summary>
        public static string AddingUnityBootstrapperExtensionToContainer {
            get {
                return ResourceManager.GetString("AddingUnityBootstrapperExtensionToContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuring module catalog..
        /// </summary>
        public static string ConfiguringModuleCatalog {
            get {
                return ResourceManager.GetString("ConfiguringModuleCatalog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuring ServiceLocator singleton..
        /// </summary>
        public static string ConfiguringServiceLocatorSingleton {
            get {
                return ResourceManager.GetString("ConfiguringServiceLocatorSingleton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuring the Unity container..
        /// </summary>
        public static string ConfiguringUnityContainer {
            get {
                return ResourceManager.GetString("ConfiguringUnityContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creating module catalog..
        /// </summary>
        public static string CreatingModuleCatalog {
            get {
                return ResourceManager.GetString("CreatingModuleCatalog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Creating Unity container..
        /// </summary>
        public static string CreatingUnityContainer {
            get {
                return ResourceManager.GetString("CreatingUnityContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one cyclic dependency has been found in the module catalog. Cycles in the module dependencies must be avoided..
        /// </summary>
        public static string CyclicDependencyFound {
            get {
                return ResourceManager.GetString("CyclicDependencyFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [{0:u}] [{1}-{3}] -&gt; {2}..
        /// </summary>
        public static string DefaultDebugLoggerPattern {
            get {
                return ResourceManager.GetString("DefaultDebugLoggerPattern", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [{0:u}] [{1}-{3}] -&gt; {2}..
        /// </summary>
        public static string DefaultTextLoggerPattern {
            get {
                return ResourceManager.GetString("DefaultTextLoggerPattern", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot add dependency for unknown module {0}.
        /// </summary>
        public static string DependencyForUnknownModule {
            get {
                return ResourceManager.GetString("DependencyForUnknownModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A module declared a dependency on another module which is not declared to be loaded. Missing module(s): {0}.
        /// </summary>
        public static string DependencyOnMissingModule {
            get {
                return ResourceManager.GetString("DependencyOnMissingModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A duplicated module with name {0} has been found by the loader..
        /// </summary>
        public static string DuplicatedModule {
            get {
                return ResourceManager.GetString("DuplicatedModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to retrieve the module type {0} from the loaded assemblies.  You may need to specify a more fully-qualified type name..
        /// </summary>
        public static string FailedToGetType {
            get {
                return ResourceManager.GetString("FailedToGetType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An exception occurred while initializing module &apos;{0}&apos;. 
        ///    - The exception message was: {2}
        ///    - The Assembly that the module was trying to be loaded from was:{1}
        ///    Check the InnerException property of the exception for more information. If the exception occurred while creating an object in a DI container, you can exception.GetRootException() to help locate the root cause of the problem. 
        ///  .
        /// </summary>
        public static string FailedToLoadModule {
            get {
                return ResourceManager.GetString("FailedToLoadModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An exception occurred while initializing module &apos;{0}&apos;. 
        ///    - The exception message was: {1}
        ///    Check the InnerException property of the exception for more information. If the exception occurred 
        ///    while creating an object in a DI container, you can exception.GetRootException() to help locate the 
        ///    root cause of the problem. .
        /// </summary>
        public static string FailedToLoadModuleNoAssemblyInfo {
            get {
                return ResourceManager.GetString("FailedToLoadModuleNoAssemblyInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to load type for module {0}. 
        ///Error was: {1}..
        /// </summary>
        public static string FailedToRetrieveModule {
            get {
                return ResourceManager.GetString("FailedToRetrieveModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing modules..
        /// </summary>
        public static string InitializingModules {
            get {
                return ResourceManager.GetString("InitializingModules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading modules..
        /// </summary>
        public static string LoadingModules {
            get {
                return ResourceManager.GetString("LoadingModules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Logger was created successfully..
        /// </summary>
        public static string LoggerCreatedSuccessfully {
            get {
                return ResourceManager.GetString("LoggerCreatedSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Module {0} depends on other modules that don&apos;t belong to the same group..
        /// </summary>
        public static string ModuleDependenciesNotMetInGroup {
            get {
                return ResourceManager.GetString("ModuleDependenciesNotMetInGroup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Module {0} was not found in the catalog..
        /// </summary>
        public static string ModuleNotFound {
            get {
                return ResourceManager.GetString("ModuleNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modules Loaded..
        /// </summary>
        public static string ModulesLoaded {
            get {
                return ResourceManager.GetString("ModulesLoaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is currently no moduleTypeLoader in the ModuleManager that can retrieve the specified module..
        /// </summary>
        public static string NoRetrieverCanRetrieveModule {
            get {
                return ResourceManager.GetString("NoRetrieverCanRetrieveModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ILoggerFacade is required and cannot be null..
        /// </summary>
        public static string NullLoggerFacadeException {
            get {
                return ResourceManager.GetString("NullLoggerFacadeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The IModuleCatalog is required and cannot be null in order to initialize the modules..
        /// </summary>
        public static string NullModuleCatalogException {
            get {
                return ResourceManager.GetString("NullModuleCatalogException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The IUnityContainer is required and cannot be null..
        /// </summary>
        public static string NullUnityContainerException {
            get {
                return ResourceManager.GetString("NullUnityContainerException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registering Framework Exception Types..
        /// </summary>
        public static string RegisteringFrameworkExceptionTypes {
            get {
                return ResourceManager.GetString("RegisteringFrameworkExceptionTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to container.
        /// </summary>
        public static string ServiceLocator_Container {
            get {
                return ResourceManager.GetString("ServiceLocator_Container", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified type is not registered: &quot;{0}&quot; with build name &quot;{1}&quot;..
        /// </summary>
        public static string ServiceLocator_NotRegistered {
            get {
                return ResourceManager.GetString("ServiceLocator_NotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Module {0} is marked for automatic initialization when the application starts, but it depends on modules that are marked as OnDemand initialization. To fix this error, mark the dependency modules for InitializationMode=WhenAvailable, or remove this validation by extending the ModuleCatalog class..
        /// </summary>
        public static string StartupModuleDependsOnAnOnDemandModule {
            get {
                return ResourceManager.GetString("StartupModuleDependsOnAnOnDemandModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provided String argument {0} must not be null or empty..
        /// </summary>
        public static string StringCannotBeNullOrEmpty {
            get {
                return ResourceManager.GetString("StringCannotBeNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type &apos;{0}&apos; was already registered by the application. Skipping....
        /// </summary>
        public static string TypeMappingAlreadyRegistered {
            get {
                return ResourceManager.GetString("TypeMappingAlreadyRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registering {0} -&gt; {1} ({2}).
        /// </summary>
        public static string UnityConventionRegistrationService_Registering {
            get {
                return ResourceManager.GetString("UnityConventionRegistrationService_Registering", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value must be of type ModuleInfo..
        /// </summary>
        public static string ValueMustBeOfTypeModuleInfo {
            get {
                return ResourceManager.GetString("ValueMustBeOfTypeModuleInfo", resourceCulture);
            }
        }
    }
}
