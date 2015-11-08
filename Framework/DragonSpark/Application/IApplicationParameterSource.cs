namespace DragonSpark.Application
{
	public interface IApplicationParameterSource
	{
		object Retrieve( ApplicationParameter parameter );
	}
}