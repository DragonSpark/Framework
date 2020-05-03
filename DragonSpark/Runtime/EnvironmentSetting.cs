using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime
{
	public sealed class EnvironmentSetting : Select<string, string?>
	{
		public static EnvironmentSetting Default { get; } = new EnvironmentSetting();

		EnvironmentSetting() : base(System.Environment.GetEnvironmentVariable) {}
	}
}