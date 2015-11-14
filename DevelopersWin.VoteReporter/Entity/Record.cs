using System.ComponentModel.DataAnnotations;

namespace DevelopersWin.VoteReporter.Entity
{
	public class Record : EntityBase
	{
		[Required]
		public virtual Recording Recording { get; set; }
		
		[Required]
		public virtual Vote Vote { get; set; }

		public int Count { get; set; }
	}
}