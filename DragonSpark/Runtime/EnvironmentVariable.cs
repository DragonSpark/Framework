using DragonSpark.Model.Results;

namespace DragonSpark.Runtime;

public class EnvironmentVariable : FixedSelectedSingleton<string, string?>
{
	public EnvironmentVariable(string name) : base(EnvironmentSetting.Default, name) {}
}