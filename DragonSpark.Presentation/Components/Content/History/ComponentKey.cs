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

	public int Get(ComponentKeyInput parameter)
	{
		var (subject, scope) = parameter;
		var hash = new HashCode();
		if (scope is string s)
		{
			hash.Add(s, _comparer);
		}
		else if (scope is not null)
		{
			hash.Add(scope);
		}

		hash.Add(_paths.Get(subject), _comparer);

		return hash.ToHashCode();
	}
}