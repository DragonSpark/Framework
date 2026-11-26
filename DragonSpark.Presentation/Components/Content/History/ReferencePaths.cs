using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class ReferencePaths : ISelect<ComponentKeyInput, ReferencePath>, IFormatter<ComponentEntryKey>, ICommand, ICommand<ComponentEntryKey>
{
	readonly NavigationManager                                  _navigation;
	readonly ICollection<string>                                _keys;
	readonly ITable<string, StandardTable<Type, List<object?>>> _references;

	public ReferencePaths(NavigationManager navigation)
		: this(navigation, new Dictionary<string, StandardTable<Type, List<object?>>>()) {}

	public ReferencePaths(NavigationManager navigation, IDictionary<string, StandardTable<Type, List<object?>>> store)
		: this(navigation, store.Keys, new References(store)) {}

	[Candidate(false)]
	public ReferencePaths(NavigationManager navigation, ICollection<string> keys, References references)
	{
		_navigation = navigation;
		_keys       = keys;
		_references = references;
	}

	public ReferencePath Get(ComponentKeyInput parameter)
	{
		var (subject, scope) = parameter;
		var type       = subject.GetType();
		var key        = $"{_navigation.Uri}#{scope}";
		var references = _references.Get(key).Get(type);
		if (!references.Contains(subject))
		{
			var index = references.FindIndex(x => x is null);
			if (index >= 0)
			{
				references[index] = subject;
			}
			else
			{
				references.Add(subject);
			}
		}

		return new(key, $"{key}+{type.FullName}+{references.IndexOf(subject)}");
	}

	public string Get(ComponentEntryKey parameter)
	{
		var (subject, scope) = parameter;
		var type       = subject.GetType();
		var references = _references.Get(scope).Get(type);
		return $"{scope}+{type.FullName}+{references.IndexOf(subject)}";
	}

	public void Execute(None parameter)
	{
		var address = _navigation.Uri;
		foreach (var key in _keys)
		{
			if (key.StartsWith(address, StringComparison.Ordinal))
			{
				_references.Get(key).Execute();
			}
		}
	}

	public void Execute(ComponentEntryKey parameter)
	{
		var (subject, scope) = parameter;
		var type       = subject.GetType();
		var references = _references.Get(scope).Get(type);
		var index      = references.IndexOf(subject);
		if (index >= 0)
		{
			references[index] = null;
		}
	}
}