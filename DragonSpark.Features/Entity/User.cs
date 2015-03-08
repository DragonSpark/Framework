using System;
using System.ComponentModel.DataAnnotations;
using DragonSpark.Application.Communication.Security;
using DragonSpark.Features.Entity.Metadata;

namespace DragonSpark.Features.Entity
{
	[MetadataType( typeof(UserMetadata) )]
	public partial class User : ApplicationUser
	{
		public string SomeStringProperty { get; set; }
		
		public DateTime? SomeDateProperty { get; set; }

		public bool SomeBooleanProperty { get; set; }
	}
}