using BenchmarkDotNet.Configs;
using DragonSpark.Compose;
using DragonSpark.Compose.Extents.Conditions;
using DragonSpark.Compose.Extents.Selections;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	public sealed class Configuration : Validated<Array<string>, IConfig>
	{
		public static Configuration Default { get; } = new Configuration();

		Configuration() : this(Start.A.Condition.Of.Type<string>().As.Sequence.Immutable,
		                       Start.A.Selection.Of.Type<string>().As.Sequence.Array) {}

		public Configuration(ConditionExtent<Array<string>> condition, SelectionExtent<Array<string>> selection)
			: base(condition.By.Calling(x => x.Length > 0),
			       selection.By.Returning(A.Result(Quick.Default)),
			       selection.By.Returning(A.Result(Deployed.Default))) {}
	}
}