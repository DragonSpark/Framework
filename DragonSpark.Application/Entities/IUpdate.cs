using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Entities
{
	public interface IUpdate<in T> : ICommand<T> where T : class {}
}