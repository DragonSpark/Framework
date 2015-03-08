using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Entity;
using DragonSpark.Objects;

namespace DragonSpark.Application.Communication.Security
{
	[RootEntitySet( DisplayNamePath = "Name", ItemName="Role", ItemNamePlural="Roles", Title="Application Roles", Order = -1 )]
	[MetadataType( typeof(RoleMetadata) )]
	public partial class Role : IObjectWithName
	{
	}

	public class RoleMetadata
	{
		[DefaultPropertyValue( "New Role" ), EntityFieldView( IsViewable = true )]
		public object Name { get; set; }
	}
}