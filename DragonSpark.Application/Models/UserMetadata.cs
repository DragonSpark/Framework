using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Models
{
	public class UserMetadata
	{
		[Display( Order = 0, Name = "Display Name" )]
		public object DisplayName { get; set; }

		[Display( Order = 0, Name = "First Name" ), Required]
		public object FirstName { get; set; }
		
		[Display( Order = 0, Name = "Last Name" )]
		public object LastName { get; set; }

		[Editable( false ), Display( Name = "Member #", Order = 1 )]
		public object MembershipNumber { get; set; }

		[Editable( false ), Display( Name = "Member Since", Order = 1 )]
		public object JoinedDate { get; set; }

		[Display( Order = 5, Name = "Some String Property" )]
		public object SomeStringProperty { get; set; }
		
		[Display( Order = 6, Name = "Some Date Property" )]
		public object SomeDateProperty { get; set; }
		
		[Display( Order = 7, Name = "Some Boolean Property" )]
		public object SomeBooleanProperty { get; set; }

		[Editable( false )]
		public object LastActivity { get; set; }

		[Display( AutoGenerateField = false )]
		public object Name { get; set; }
	}
}