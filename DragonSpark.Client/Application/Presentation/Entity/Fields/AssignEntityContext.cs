using System.ServiceModel.DomainServices.Client;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.ComponentModel;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class AssignEntityContext : ViewObject
	{
		readonly DomainContext context;
		readonly IEntitySetProfile profile;
		readonly string viewProfileName;

		public AssignEntityContext( DomainContext context, IEntitySetProfile profile, string viewProfileName )
		{
			this.context = context;
			this.profile = profile;
			this.viewProfileName = viewProfileName;
		}

		public DomainContext Context
		{
			get { return context; }
		}

		public IEntitySetProfile Profile
		{
			get { return profile; }
		}

		public string ViewProfileName
		{
			get { return viewProfileName; }
		}

		public string QuickFilter
		{
			get { return quickFilter; }
			set { SetProperty( ref quickFilter, value, () => QuickFilter ); }
		}	string quickFilter;

		public System.ServiceModel.DomainServices.Client.Entity SelectedEntity
		{
			get { return selectedEntity; }
			set { SetProperty( ref selectedEntity, value, () => SelectedEntity ); }
		}	System.ServiceModel.DomainServices.Client.Entity selectedEntity;
	}
}