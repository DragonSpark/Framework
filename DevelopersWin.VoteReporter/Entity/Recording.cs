using DragonSpark.ComponentModel;
using System.Collections.Generic;

namespace DevelopersWin.VoteReporter.Entity
{
	public class Recording : EntityBase
	{
		
		[Collection]
		public virtual ICollection<Record> Records { get; set; }
	}
}