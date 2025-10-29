using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime;

public class EnvironmentVariable : FixedSelectedSingleton<string, string?>
{
    public static implicit operator string(EnvironmentVariable @this) => @this.Get().Verify();

	public EnvironmentVariable(string name) : base(EnvironmentSetting.Default, name) {}
}