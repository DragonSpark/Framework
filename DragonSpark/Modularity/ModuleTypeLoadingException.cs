using System;
using System.Globalization;
using DragonSpark.Properties;

namespace DragonSpark.Modularity
{
    /// <summary>
    /// Exception thrown by <see cref="IModuleManager"/> implementations whenever 
    /// a module fails to retrieve.
    /// </summary>
    public partial class ModuleTypeLoadingException : ModularityException
    {
        /// <summary>
        /// Initializes the exception with a particular module, error message and inner exception that happened.
        /// </summary>
        /// <param name="moduleName">The name of the module.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a <see langword="null"/> reference if no inner exception is specified.</param>
        public ModuleTypeLoadingException(string moduleName, string message, Exception innerException)
            : base(moduleName, String.Format(CultureInfo.CurrentCulture, Resources.FailedToRetrieveModule, moduleName, message), innerException)
        {
        }
    }
}