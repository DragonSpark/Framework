using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Application.Entities.Editing
{
	public interface IEditor : IOperation, IDisposable
	{
		void Add(object entity);

		void Attach(object entity);

		void Update(object entity);

		void Remove(object entity);
	}
}