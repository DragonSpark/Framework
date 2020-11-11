using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Entities
{
	public interface IUndo : ICommand<object>, ICommand {}
}