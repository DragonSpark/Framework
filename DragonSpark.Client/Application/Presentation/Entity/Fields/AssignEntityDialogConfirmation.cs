using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class AssignEntityDialogConfirmation : Confirmation
	{
		public AssignEntityDialogConfirmation( string title, AssignEntityContext context )
		{
			Content = context;
			Title = title;
		}
	}
}