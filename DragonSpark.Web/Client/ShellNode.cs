namespace DragonSpark.Web.Client
{
	public class ShellNode : NavigationNode
	{
		public string TransitionName { get; set; }

		public NavigationNode NotFound { get; set; }
	}
}