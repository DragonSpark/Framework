using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class ModifyMany<T> : IOperation<Array<T>> where T : class
{
	readonly SessionEditors _editors;
	readonly IModify<T>     _modify;

	protected ModifyMany(SessionEditors editors, IModify<T> modify)
	{
		_editors = editors;
		_modify  = modify;
	}

	public async ValueTask Get(Array<T> parameter)
	{
		using var editor = await _editors.Await();
		foreach (var item in parameter.Open())
		{
			_modify.Execute(new(editor, item));
		}
	}
}