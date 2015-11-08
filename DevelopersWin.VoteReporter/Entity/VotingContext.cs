using DragonSpark.Windows.Entity;
using System.Data.Entity;

namespace DevelopersWin.VoteReporter.Entity
{
	public class VotingContext : EntityContext
	{
		public VotingContext()
		{
		}

		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			// modelBuilder.Entity<Vote>().HasOptional(  )

			base.OnModelCreating( modelBuilder );
		}

		public DbSet<VoteGroup> Groups { get; set; }

		public DbSet<Vote> Votes { get; set; }

		public DbSet<VoteRecording> Recordings { get; set; }
	}
}