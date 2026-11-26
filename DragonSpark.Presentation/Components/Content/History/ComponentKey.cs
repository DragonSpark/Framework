namespace DragonSpark.Presentation.Components.Content.History;

sealed class ComponentKey : IComponentKey
{
	readonly ReferencePaths _paths;
	readonly StringComparer _comparer;

	public ComponentKey(ReferencePaths paths) : this(paths, StringComparer.Ordinal) {}

	public ComponentKey(ReferencePaths paths, StringComparer comparer)
	{
		_paths    = paths;
		_comparer = comparer;
	}

	public ComponentKeyHash Get(ComponentKeyInput parameter)
	{
		var (scope, key) = _paths.Get(parameter);
		var hash = _comparer.GetHashCode(key);
		return new(scope, hash);
	}

	public void Execute(ComponentEntryKey parameter)
	{
		_paths.Execute(parameter);
	}

	public int Get(ComponentEntryKey parameter) => _comparer.GetHashCode(_paths.Get(parameter));
}