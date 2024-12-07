using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Configuration;

sealed class AssignSetting : IOperation<Pair<string, string?>>
{
	readonly EditSetting _edit;

	public AssignSetting(EditSetting edit) => _edit = edit;

	public async ValueTask Get(Pair<string, string?> parameter)
	{
		var (key, value) = parameter;
		using var edit    = await _edit.Await(key);
		var       subject = edit.Subject ?? new() { Id = key };
		subject.Value = value;
		edit.Update(subject);
		await edit.Await();
	}
}