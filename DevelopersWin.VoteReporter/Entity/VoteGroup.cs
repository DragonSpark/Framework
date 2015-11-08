using System.Collections.Generic;
using System.Windows.Markup;
using DragonSpark.ComponentModel;

namespace DevelopersWin.VoteReporter.Entity
{
	[ContentProperty( "Votes" )]
	public class VoteGroup : VoteBase
	{
		[Collection]
		public virtual ICollection<Vote> Votes { get; set; }
	}
}