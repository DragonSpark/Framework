using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Testing.Objects
{
	sealed class Near : Instance<Selection>
	{
		public static Near Default { get; } = new Near();

		Near() : base(new Selection(300, 100)) {}
	}
}