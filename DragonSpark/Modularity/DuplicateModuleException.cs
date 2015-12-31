namespace DragonSpark.Modularity
{
	/// <summary>
	/// Exception thrown when a module is declared twice in the same catalog.
	/// </summary>
	public partial class DuplicateModuleException : ModularityException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DuplicateModuleException" /> class with a specified error message.
		/// </summary>
		/// <param name="moduleName">The name of the module.</param>
		/// <param name="message">The message that describes the error.</param>
		public DuplicateModuleException(string moduleName, string message)
			: base(moduleName, message)
		{}
	}
}
