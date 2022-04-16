using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class MarkManyForRemoval<T> : IOperation<ICollection<T>> where T : class
{
	readonly SessionEditors _editors;
	readonly IModify<T>        _modify;

	public MarkManyForRemoval(SessionEditors editors) : this(editors, RemoveLocal<T>.Default) {}

	public MarkManyForRemoval(SessionEditors editors, IModify<T> modify)
	{
		_editors = editors;
		_modify       = modify;
	}

	public async ValueTask Get(ICollection<T> parameter)
	{
		using var editor      = await _editors.Await();
		foreach (var item in parameter.AsValueEnumerable())
		{
			_modify.Execute(new Edit<T>(editor, item));
		}
	}
}