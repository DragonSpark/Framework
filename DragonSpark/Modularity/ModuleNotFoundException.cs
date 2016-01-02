namespace DragonSpark.Modularity
{
	/// <summary>
	/// Exception thrown when a requested <see cref="InitializationMode.OnDemand"/> <see cref="IModule"/> was not found.
	/// </summary>
	public partial class ModuleNotFoundException : ModularityException
	{
	  
		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleNotFoundException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="moduleName">The name of the module.</param>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public ModuleNotFoundException(string moduleName, string message)
			: base(moduleName, message)
		{
		}

		
	}
}
