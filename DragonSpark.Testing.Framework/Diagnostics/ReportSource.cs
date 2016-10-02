using System;
using System.Collections.Immutable;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Testing.Framework.Diagnostics
{
	class ReportSource : ParameterizedSourceBase<ReportSource.Parameter, ImmutableArray<ReportSource.Result>>
	{
		public static ReportSource Default { get; } = new ReportSource();

		public override ImmutableArray<Result> Get( Parameter parameter ) => 
			parameter.Actions.Introduce( parameter, tuple => new Run( tuple.Item1, tuple.Item2.NumberOfRuns, tuple.Item2.PerRun ).Get() ).ToImmutableArray();

		public struct Parameter
		{
			public Parameter( ImmutableArray<Action> actions, int numberOfRuns = 100, int perRun = 10000 )
			{
				Actions = actions;
				NumberOfRuns = numberOfRuns;
				PerRun = perRun;
			}

			public ImmutableArray<Action> Actions { get; }
			public int NumberOfRuns { get; }
			public int PerRun { get; }
		}

		public struct Result
		{
			public Result( string name, TimeSpan average, TimeSpan median, TimeSpan mode )
			{
				Name = name;
				Average = average;
				Median = median;
				Mode = mode;
			}

			public string Name { get; }
			public TimeSpan Average { get; }
			public TimeSpan Median { get; }
			public TimeSpan Mode { get; }
		}
	}
}