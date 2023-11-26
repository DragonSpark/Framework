using DragonSpark.Application.Entities.Editing;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Configuration;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ISettingAccessor>()
		         .Forward<SettingAccessor>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton();
	}
}

public interface ISettingAccessor : ISelecting<string, string?>, IOperation<Pair<string, string?>>
{
	IRemove Remove { get; }
}

public interface IRemove : IOperation<string>;

public interface ISettingVariable : IResulting<string?>, IOperation<string>
{
	IOperation Remove { get; }
}

public class SettingVariable : ISettingVariable
{
	readonly string           _key;
	readonly ISettingAccessor _store;

	protected SettingVariable(string key, ISettingAccessor store)
		: this(key, store, store.Remove.Then().Bind(key).Out()) {}

	protected SettingVariable(string key, ISettingAccessor store, IOperation remove)
	{
		_key   = key;
		_store = store;
		Remove = remove;
	}

	public async ValueTask<string?> Get()
	{
		var result = await _store.Get(_key).ConfigureAwait(false);
		return result;
	}

	public ValueTask Get(string parameter) => _store.Get((_key, parameter));

	public IOperation Remove { get; }
}

sealed class SelectSetting : StartWhere<string, Setting>
{
	public static SelectSetting Default { get; } = new();

	SelectSetting() : base((p, x) => x.Id == p) {}
}

sealed class GetSetting : EvaluateToSingleOrDefault<string, Setting>
{
	public GetSetting(IStandardScopes scopes) : base(scopes, SelectSetting.Default) {}
}

/*sealed class DetermineSetting : Coalesce<string, Setting>
{
	public DetermineSetting(GetSetting first, NewSetting second) : base(first, second) {}
}*/

/*sealed class NewSetting : ISelecting<string, Setting>
{
	public static NewSetting Default { get; } = new();

	NewSetting() {}

	public ValueTask<Setting> Get(string parameter) => new Setting(parameter, null).ToOperation();
}*/

sealed class SettingAccessor : Operation<Pair<string, string?>>, ISettingAccessor
{
	readonly GetSetting _setting;

	public SettingAccessor(AssignSetting assign, GetSetting setting, RemoveSetting remove) : base(assign)
	{
		Remove   = remove;
		_setting = setting;
	}

	public IRemove Remove { get; }

	public async ValueTask<string?> Get(string parameter)
	{
		var setting = await _setting.Await(parameter);
		return setting?.Value;
	}
}

sealed class AssignSetting : IOperation<Pair<string, string?>>
{
	readonly EditSetting _edit;

	public AssignSetting(EditSetting edit) => _edit = edit;

	public async ValueTask Get(Pair<string, string?> parameter)
	{
		var (key, value) = parameter;
		using var edit    = await _edit.Await(key);
		var       subject = edit.Subject ?? new(key, value);
		edit.Update(subject with { Value = value });
		await edit.Await();
	}
}

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

sealed class EditSetting : EditOrDefault<string, Setting>
{
	public EditSetting(IEnlistedScopes context) : base(context, SelectSetting.Default) {}
}