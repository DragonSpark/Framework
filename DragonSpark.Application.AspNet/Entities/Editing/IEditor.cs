using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public interface IEditor : IOperation, IDisposable
{
	void Add(object entity);

	void Attach(object entity);

	void Update(object entity);

	void Remove(object entity);

	void Clear();

	ValueTask Refresh(object entity);
}