using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Communication.Security
{
	[DisplayColumn( "Name", "Order" )]
	public partial class Role
	{
		[Key]
		public string Name { get; set; }

		public int? Order { get; set; }
	}
}