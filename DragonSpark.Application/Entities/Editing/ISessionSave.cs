using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing
{
	public interface ISessionSave<in T> : IOperation<T> {}
}