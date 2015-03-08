using System.ComponentModel;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public interface IRemovalDialog
    {
        bool? CanRemove( IRevertibleChangeTracking currentItem );
    }
}