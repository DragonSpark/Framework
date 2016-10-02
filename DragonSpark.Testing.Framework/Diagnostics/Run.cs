using DragonSpark.Sources;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Testing.Framework.Diagnostics
{
	sealed class Run : SourceBase<ReportSource.Result>
	{
		readonly Action action;
		readonly int numberOfSamples;
		readonly int perSample;
				
		public Run( Action action, int numberOfSamples, int perSample )
		{
			this.action = action;
			this.numberOfSamples = numberOfSamples;
			this.perSample = perSample;
		}

		public override ReportSource.Result Get()
		{
			var data = EnumerableEx.Generate( 0, Continue, i => i + 1, Measure ).Select( span => span.Ticks ).ToArray();
			var average = data.Average( span => span );
			var median = MedianFactory.Default.Get( data.ToImmutableArray() );
			var mode = ModeFactory<long>.Default.Get( data.ToImmutableArray() );
			var result = new ReportSource.Result( action.Method.Name, TimeSpan.FromTicks( (long)average ), TimeSpan.FromTicks( median ), TimeSpan.FromTicks( mode ) );
			return result;
		}

		bool Continue( int i ) => i < numberOfSamples;

		TimeSpan Measure<T>( T _ )
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			for ( var i = 0; i < perSample; i++ )
			{
				action();
			}
			watch.Stop();
			var result = watch.Elapsed;
			return result;
		}
	}
}