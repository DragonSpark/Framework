using DragonSpark.Sources.Parameterized;

namespace DragonSpark.ComponentModel
{
	public interface IDefaultValueProvider : IParameterizedSource<DefaultValueParameter, object>
	{
		// object GetValue( DefaultValueParameter parameter );
	}
}