using DragonSpark.Security;
using System;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Server.Models
{
	[MetadataType( typeof(UserMetadata) )]
	public class ApplicationUserProfile : UserProfile
	{
		public string SomeStringProperty { get; set; }
		
		public DateTime? SomeDateProperty { get; set; }

		public bool SomeBooleanProperty { get; set; }
	}
}