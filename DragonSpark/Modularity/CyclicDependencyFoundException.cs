namespace DragonSpark.Modularity
{
	/// <summary>
	/// Represents the exception that is thrown when there is a circular dependency
	/// between modules during the module loading process.
	/// </summary>
	public partial class CyclicDependencyFoundException : ModularityException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CyclicDependencyFoundException"/> class
		/// with the specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public CyclicDependencyFoundException(string message) : base(message) { }

		
	}
}