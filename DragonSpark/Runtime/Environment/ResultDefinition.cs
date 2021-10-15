using DragonSpark.Model.Results;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Environment;

sealed class ResultDefinition : MakeGenericType
{
	public static ResultDefinition Default { get; } = new ResultDefinition();

	ResultDefinition() : base(typeof(IResult<>)) {}
}