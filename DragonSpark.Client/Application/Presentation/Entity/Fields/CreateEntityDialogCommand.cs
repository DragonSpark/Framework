using DragonSpark.Objects;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class CreateEntityDialogCommand : EntityDialogCommandBase
	{
		[DefaultPropertyValue( "Create New Entity" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override System.ServiceModel.DomainServices.Client.Entity DetermineEntity( EntityReferenceField parameter )
		{
			var result = Activator.CreateInstance<System.ServiceModel.DomainServices.Client.Entity>( parameter.EntityType );
			return result;
		}

		protected override void OnCommit( EntityReferenceField entityReferenceField, System.ServiceModel.DomainServices.Client.Entity entity )
		{
			entityReferenceField.Add( entity );
		}
	}
}