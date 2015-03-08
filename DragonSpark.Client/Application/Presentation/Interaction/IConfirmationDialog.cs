namespace DragonSpark.Application.Presentation.Interaction
{
	public interface IConfirmationDialog
	{
		bool? Confirm( object state = null );
	}
}