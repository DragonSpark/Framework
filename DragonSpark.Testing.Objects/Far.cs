using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Testing.Objects
{
	sealed class Far : Instance<Selection>
	{
		public static Far Default { get; } = new Far();

		Far() : base(new Selection(5000, 300)) {}
	}
}