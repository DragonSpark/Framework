using System.ComponentModel;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
	public interface IPreserveChangesDialog
	{
		bool? CanDismissChanges( IRevertibleChangeTracking currentItem );
	}
}