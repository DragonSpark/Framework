﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not resolve type &quot;{0}&quot; with build name &quot;{1}&quot;.  Details: {2}.
        /// </summary>
        internal static string Activator_CouldNotActivate {
            get {
                return ResourceManager.GetString("Activator_CouldNotActivate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;None&gt;.
        /// </summary>
        internal static string Activator_None {
            get {
                return ResourceManager.GetString("Activator_None", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to container.
        /// </summary>
        internal static string ServiceLocator_Container {
            get {
                return ResourceManager.GetString("ServiceLocator_Container", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified type is not registered: &quot;{0}&quot; with build name &quot;{1}&quot;..
        /// </summary>
        internal static string ServiceLocator_NotRegistered {
            get {
                return ResourceManager.GetString("ServiceLocator_NotRegistered", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Registering {0} -&gt; {1}.
        /// </summary>
        internal static string UnityConventionRegistrationService_Registering {
            get {
                return ResourceManager.GetString("UnityConventionRegistrationService_Registering", resourceCulture);
            }
        }
    }
}
