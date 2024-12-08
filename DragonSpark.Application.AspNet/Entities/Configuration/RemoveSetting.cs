using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

sealed class RemoveSetting : IRemove
{
	readonly EditSetting _edit;

	public RemoveSetting(EditSetting edit) => _edit = edit;

	public async ValueTask Get(string parameter)
	{
		using var edit = await _edit.Await(parameter);
		if (edit.Subject is not null)
		{
			edit.Remove(edit.Subject);
			await edit.Await();
		}
	}
}