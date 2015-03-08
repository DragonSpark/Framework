namespace DragonSpark.Application.Presentation.Interaction
{
	public class GoToStateContext
	{
		public string TargetName { get; set; }

		public string StateName { get; set; }

		public bool UseTransitions { get; set; }
	}
}