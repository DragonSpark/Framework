using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Testing.Framework.Diagnostics
{
	public class PerformanceSupport
	{
		readonly static string[] Titles = { "Test", "Average", "Median", "Mode" };
		const string TimeFormat = "ss'.'ffff";

		readonly Action<string> output;
		readonly ImmutableArray<Action> actions;

		public PerformanceSupport( Action<string> output, params Action[] actions )
		{
			this.output = output;
			this.actions = actions.ToImmutableArray();

			foreach ( var action in actions )
			{
				action();
			}
		}

		public void Run( int numberOfRuns = 100, int perRun = 10000 )
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();

			var results = ReportSource.Default.Get( new ReportSource.Parameter( actions, numberOfRuns, perRun ) ).ToArray();

			var max = results.Max( r => r.Name.Length );
			var template = $"{{0,-{max}}} | {{1, 7}} | {{2, 7}} | {{3, 7}}";

			var title = string.Format( template, Titles );
			output( title );
			output( new string( '-', title.Length ) );

			foreach ( var result in results )
			{
				output( string.Format( template, result.Name, result.Average.ToString( TimeFormat ), result.Median.ToString( TimeFormat ), result.Mode.ToString( TimeFormat ) ) );
			}
		}
	}
}
