using System.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class EditEntityDialogCommand : EntityDialogCommandBase
	{
		[DefaultPropertyValue( "Edit Existing Entity" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override System.ServiceModel.DomainServices.Client.Entity DetermineEntity( EntityReferenceField parameter )
		{
			var result = parameter.Value;
			return result;
		}

		protected override void OnCommit( EntityReferenceField parameter, System.ServiceModel.DomainServices.Client.Entity entity )
		{
			parameter.ApplyEdit();
		}

		protected override void OnCancel( EntityReferenceField parameter, System.ServiceModel.DomainServices.Client.Entity entity )
		{
			entity.As<IRevertibleChangeTracking>( x => x.RejectChanges() );
		}
	}
}