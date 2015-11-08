using DragonSpark.ComponentModel;
using System.Collections.Generic;

namespace DevelopersWin.VoteReporter.Entity
{
	public class VoteRecording : EntityBase
	{
		
		[Collection]
		public virtual ICollection<VoteRecord> Records { get; set; }
	}
}