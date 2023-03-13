using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public sealed class Clear : IOperation
{
	readonly SessionEditors _editors;

	public Clear(SessionEditors editors) => _editors = editors;

	public async ValueTask Get()
	{
		using var editor = await _editors.Await();
		editor.Clear();
	}
}