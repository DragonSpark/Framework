namespace DragonSpark.Server.Client
{
	public class ShellNode : NavigationNode
	{
		public string TransitionName { get; set; }

		public NavigationNode NotFound { get; set; }
	}
}