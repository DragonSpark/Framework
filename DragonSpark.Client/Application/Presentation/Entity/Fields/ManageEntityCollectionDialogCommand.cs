using System.Windows.Controls;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
    public class ManageEntityCollectionDialogCommand : DialogCommand<EntityCollectionField, ManageEntityCollectionNotification>
    {
        [DefaultPropertyValue( "Manage Entity Collection" )]
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

        protected override void Execute( EntityCollectionField parameter )
        {
            var context = new ManageEntityCollectionContext( parameter.Value.CreateView(), parameter.Profile, parameter.ButtonsVisibility & ~DataFormCommandButtonsVisibility.Commit, ViewProfileName );
            DisplayRequest.Raise( new ManageEntityCollectionNotification( Title, context ), x => parameter.Assign( context.View ) );
        }
    }
}