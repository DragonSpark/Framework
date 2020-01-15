namespace DragonSpark.Runtime.Environment
{
	sealed class PrimaryAssemblyMessage : Text.Text
	{
		public static PrimaryAssemblyMessage Default { get; } = new PrimaryAssemblyMessage();

		PrimaryAssemblyMessage()
			: base("A request was made for this application's primary assembly, but one could not be located.  Please ensure the entry or primary assembly or executable used for this application is marked with an attribute that inherits from DragonSpark.Runtime.Environment.HostingAttribute.") {}
	}
}