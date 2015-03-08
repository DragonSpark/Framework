namespace DragonSpark.Application
{
	/// <summary>
	///     Controls when a stack trace should be displayed on the
	///     Error Window
	///     
	///     Defaults to <see cref="OnlyWhenDebuggingOrRunningLocally"/>
	/// </summary>
	public enum StackTracePolicy
	{
		/// <summary>
		///   Stack trace is showed only when running with a debugger attached
		///   or when running the app on the local machine. Use this to get
		///   additional debug information you don't want the end users to see
		/// </summary>
		OnlyWhenDebuggingOrRunningLocally,

		/// <summary>
		/// Always show the stack trace, even if debugging
		/// </summary>
		Always,

		/// <summary>
		/// Never show the stack trace, even when debugging
		/// </summary>
		Never
	}
}