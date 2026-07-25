using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public interface IEditor : IOperation, IDisposable
{
	void Add(object entity);

	void Attach(object entity);

	void Update(object entity);

	void Remove(object entity);

	void Clear();

	ValueTask Refresh(object entity);
}