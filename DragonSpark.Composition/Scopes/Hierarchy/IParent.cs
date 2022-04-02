using DragonSpark.Model.Results;

namespace DragonSpark.Composition.Scopes.Hierarchy;

public interface IParent<out T> : IResult<T> where T : notnull
{
	
}