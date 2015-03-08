using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Communication.Entity.Notifications
{
	[Singleton( typeof(INotificationProcessorLocator), Priority = Priority.Lowest )]
	public class MetadataNotificationProcessorLocator : INotificationProcessorLocator
	{
		public INotificationProcessor Locate( INotification applicationRequest )
		{
			var result = applicationRequest.FromMetadata<NotificationProcessorAttribute, INotificationProcessor>( x => ServiceLocator.Current.GetInstance( x.ProcessorType, x.Name ).As<INotificationProcessor>() );
			return result;
		}
	}
}