namespace DragonSpark.Web.Client
{
	public class ClientModule
	{
		public string Path { get; set; }
	}

	public class WidgetModule : ClientModule
	{
		public string Name { get; set; }
	}
}