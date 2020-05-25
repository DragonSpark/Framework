using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public interface IResulting<T> : IResult<ValueTask<T>> {}
}