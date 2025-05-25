using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

sealed class AssignSetting : IStopAware<Pair<string, string?>>
{
	readonly EditSetting _edit;

	public AssignSetting(EditSetting edit) => _edit = edit;

	public async ValueTask Get(Stop<Pair<string, string?>> parameter)
	{
		var ((key, value), stop) = parameter;
		using var edit    = await _edit.Off(new(key, stop));
		var       subject = edit.Subject ?? new() { Id = key };
		subject.Value = value;
		edit.Update(subject);
		await edit.Off();
	}
}