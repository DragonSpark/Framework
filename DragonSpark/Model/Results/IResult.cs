namespace DragonSpark.Model.Results;

public interface IResult<out T>
{
	T Get();
}