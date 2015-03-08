using DragonSpark.Application.Presentation.Infrastructure;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
    public class ManageEntityCollectionNotification : BasicNotification
    {
        public ManageEntityCollectionNotification( string title, ManageEntityCollectionContext context  ) : base( title )
        {
            Content = context;
            Title = title;
        }
    }
}