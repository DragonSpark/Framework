using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities
{
	public interface IStorageState : IResulting<int>, ICommand<object> {}
}