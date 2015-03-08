using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Entity;
using DragonSpark.Application.Presentation.Entity.Interactivity;

namespace DragonSpark.Features.Entity
{
	[EntityDataBehavior( typeof(DisplayNameEntityDataBehavior) )]
	[MetadataType( typeof(UserMetadata) )]
	[RootEntitySet( DisplayNamePath = "DisplayName", ItemName = "User", ItemNamePlural = "Users", Title = "Application User Accounts", AuthorizedRoles = "Administrator", Order = -2 )]
	public partial class User
	{
		public class DisplayNameEntityDataBehavior : EntityDataBehaviorBase<User>
		{
			protected override string[]  ResolveProperties()
			{
				var result = new[] { "FirstName", "LastName" };
				return result;
			}

			protected override void Apply( string propertyName )
			{
				AssociatedEntity.DisplayName = string.Concat( AssociatedEntity.FirstName, string.IsNullOrEmpty( AssociatedEntity.LastName ) ? string.Empty : " ", AssociatedEntity.LastName );
			}
		}
	}

	public class UserMetadata
	{
		[EntityFieldView( "Administration", AuthorizedRoles = "Administrator", ModelType = typeof(RolesField) ), EntityFieldView( IsViewable = false )]
		public object RolesSource { get; set; }

		[EntityFieldView( IsViewable = false )]
		public object Claims { get; set; }

		[Editable( false ), EntityFieldView( "Administration", AuthorizedRoles = "Administrator" ), EntityFieldView( IsViewable = false )]
		public object LastActivity { get; set; }

		[Editable( false ), EntityFieldView( "Administration", AuthorizedRoles = "Administrator" ), EntityFieldView( IsViewable = false )]
		public object LastSynchronized { get; set; }

		[EntityFieldView( IsViewable = false )]
		public object IsAuthenticated { get; set; }

		[EntityFieldView( IsEditable = false )]
		public object DisplayName { get; set; }

		[EntityFieldView( IsViewable = false )]
		[EntityFieldView( "Administration", AuthorizedRoles = "Administrator" )]
		public object IsEnabled { get; set; }
	}
}