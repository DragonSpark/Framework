namespace DragonSpark.ComponentModel
{
	public interface IDefaultValueProvider
	{
		object GetValue( DefaultValueParameter parameter );
	}
}