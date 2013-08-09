using System;
using System.ComponentModel.DataAnnotations;
using DragonSpark.Security;

namespace DragonSpark.Application.Models
{
	[MetadataType( typeof(UserMetadata) )]
	public class ApplicationUserProfile : UserProfile
	{
		public string SomeStringProperty { get; set; }
		
		public DateTime? SomeDateProperty { get; set; }

		public bool SomeBooleanProperty { get; set; }
	}
}