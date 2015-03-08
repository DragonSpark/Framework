using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Entity.Interactivity;

namespace DragonSpark.Features.Entity
{
	[EntityDataBehavior( typeof(DisplayNameEntityDataBehavior) )]
	[MetadataType( typeof(UserMetadata) )]
	[RootEntitySet( DisplayNamePath = "DisplayName", ItemName = "Sample User", ItemNamePlural = "Sample Users", Title = "Sample User Accounts", Order = -2 )]
	public partial class SampleUser
	{
		public class DisplayNameEntityDataBehavior : EntityDataBehaviorBase<SampleUser>
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
}