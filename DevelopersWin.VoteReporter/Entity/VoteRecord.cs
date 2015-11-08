using System.ComponentModel.DataAnnotations;

namespace DevelopersWin.VoteReporter.Entity
{
	public class VoteRecord : EntityBase
	{
		[Required]
		public virtual VoteRecording Set { get; set; }
		
		[Required]
		public virtual Vote Vote { get; set; }

		public int Count { get; set; }
	}
}