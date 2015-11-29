using DevelopersWin.VoteReporter.Entity;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevelopersWin.VoteReporter
{
	public class VoteReportFactory : FactoryBase<VoteReport>
	{
		readonly VotingContext context;

		public VoteReportFactory( VotingContext context )
		{
			this.context = context;
		}

		protected override VoteReport CreateItem()
		{
			var recordings = context.Recordings.OrderByDescending( recording => recording.Created );
			var current = recordings.First();
		    var firstOrDefault = recordings.FirstOrDefault( recording => recording.Created < current.Created );
		    var previous = firstOrDefault.Transform( recording => Convert( recording, null ) );
			var result = Convert( current, previous );
			return result;
		}

		static VoteReport Convert( Recording source, VoteReport reference )
		{
			var result = source.MapInto<VoteReport>().With( report => report.Groups.AddRange( DetermineGroups( source, reference ) ) );
			return result;
		}

		static IEnumerable<VoteGroupView> DetermineGroups( Recording current, VoteReport previous )
		{
			var groups = current.Records.GroupBy( record => record.Vote.Group ).OrderBy( grouping => grouping.Key.Order ).Select( records => records.Key );
			var result = groups.Select( @group => Create( current, @group, previous.Transform( x => x.Groups.SingleOrDefault( y => y.Id == @group.Id ) ) ) ).ToArray();
			return result;
		}

		static VoteGroupView Create( Recording recording, VoteGroup current, VoteGroupView previous )
		{
			var result = current.MapInto<VoteGroupView>();
			var count = current.Votes.Sum( vote => vote.Records.SingleOrDefault( record => record.Recording == recording ).Count );
			result.Counts = new VoteCount { Count = count, Delta = count - previous.Transform( view => view.Counts.Count ) }.BuildUp();
			result.Votes.AddRange( current.Votes.OrderBy( vote => vote.Order ).Select( v => CreateVote( recording, v, previous.Transform( x => x.Votes.SingleOrDefault( y => y.Id == v.Id ) ) ) ) );
			return result;
		}

		static VoteView CreateVote( Recording recording, Vote current, VoteView previous )
		{
			var result = current.MapInto<VoteView>();
			var count = current.Records.SingleOrDefault( record => record.Recording == recording ).Count;
			result.Counts = new VoteCount { Count = count, Delta = count - previous.Transform( view => view.Counts.Count ) }.BuildUp();
			return result;
		}
	}

	public class VoteReport : ViewBase
	{
		public Collection<VoteGroupView> Groups { get; } = new Collection<VoteGroupView>();
	}

	public abstract class VoteViewBase : ViewBase
	{
		public string Title { get; set; }

		public VoteCount Counts { get; set; }
	}

	public class VoteCount : ViewBase
	{
		public int Count { get; set; }

		public int Delta { get; set; }
	}

	public class VoteGroupView : VoteViewBase
	{
		public Collection<VoteView> Votes { get; } = new Collection<VoteView>();
	}

	public class VoteView : VoteViewBase
	{
		public Uri Location { get; set; }
	}
}