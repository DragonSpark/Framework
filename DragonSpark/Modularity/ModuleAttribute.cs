

using System;

namespace DragonSpark.Modularity
{
     /// <summary>
    /// Indicates that the class should be considered a named module using the
    /// provided module name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }
    }
}
