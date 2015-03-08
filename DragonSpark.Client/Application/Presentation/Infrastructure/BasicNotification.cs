using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public class BasicNotification : Notification
    {
        public BasicNotification( string title )
        {
            Title = title;
            Content = this;
        }
    }
}