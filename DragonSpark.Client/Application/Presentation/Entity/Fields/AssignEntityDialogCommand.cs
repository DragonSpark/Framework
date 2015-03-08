using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class AssignEntityDialogCommand : DialogCommand<EntityReferenceField, AssignEntityDialogConfirmation>
	{
		[DefaultPropertyValue( "Assign Existing Entity Reference" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[DefaultPropertyValue( "Administration" )]
		public string ViewProfileName
		{
			get { return viewProfileName; }
			set { SetProperty( ref viewProfileName, value, () => ViewProfileName ); }
		}	string viewProfileName;

		protected override void Execute( EntityReferenceField parameter )
		{
			var context = new AssignEntityContext( parameter.Context, parameter.Profile, ViewProfileName );
			DisplayRequest.Raise( new AssignEntityDialogConfirmation( Title, context ), x =>
			{
				var assign = context.SelectedEntity != null && x.Confirmed;
				assign.IsTrue( () => parameter.Assign( context.SelectedEntity ) );
			} );
		}
	}
}