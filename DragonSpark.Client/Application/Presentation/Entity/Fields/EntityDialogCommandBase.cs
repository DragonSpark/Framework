using System;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public abstract class EntityDialogCommandBase : DialogCommand<EntityReferenceField, EntityDialogNotification>
	{
		[DefaultPropertyValue( "Administration" )]
		public string ViewProfileName
		{
			get { return viewProfileName; }
			set { SetProperty( ref viewProfileName, value, () => ViewProfileName ); }
		}	string viewProfileName;

		protected override void Execute( EntityReferenceField parameter )
		{
			var item = DetermineEntity( parameter );
			DisplayRequest.Raise( new EntityDialogNotification( item, Title, ViewProfileName ), x => 
			{
				var action = x.Result ? (Action<EntityReferenceField, System.ServiceModel.DomainServices.Client.Entity>)OnCommit : OnCancel;
				action( parameter, item );
			} );
		}

		protected virtual void OnCommit( EntityReferenceField parameter, System.ServiceModel.DomainServices.Client.Entity entity )
		{}

		protected virtual void OnCancel( EntityReferenceField parameter, System.ServiceModel.DomainServices.Client.Entity entity )
		{}

		protected abstract System.ServiceModel.DomainServices.Client.Entity DetermineEntity( EntityReferenceField parameter );
	}
}