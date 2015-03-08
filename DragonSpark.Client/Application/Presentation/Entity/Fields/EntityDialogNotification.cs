using DragonSpark.Application.Presentation.Infrastructure;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class EntityDialogNotification : BasicNotification
	{
		public EntityDialogNotification( System.ServiceModel.DomainServices.Client.Entity entity, string title, string profileName ) : base( title )
		{
			Entity = entity;
			ProfileName = profileName;
		}

		public System.ServiceModel.DomainServices.Client.Entity Entity { get; private set; }
		public string ProfileName { get; private set; }

		public bool Result { get; set; }
	}
}