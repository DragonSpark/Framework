using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class FieldRegistry
{
	readonly Dictionary<string, FieldIdentifier> _identifiers;

	public FieldRegistry() : this(new Dictionary<string, FieldIdentifier>()) {}

	public FieldRegistry(Dictionary<string, FieldIdentifier> identifiers) => _identifiers = identifiers;

	/*public void Clear(Expression<Func<object?>> identifier)
	{
		_identifiers.Remove(identifier.ToString());
	}*/

	public FieldIdentifier Register<T>(Expression<Func<T>> identifier)
	{
		var key = identifier.ToString();
		return _identifiers.TryGetValue(key, out var existing)
			       ? existing
			       : Add(key, FieldIdentifier.Create(identifier));
	}

	public FieldIdentifier Register(object model, string path)
	{
		var key = $"{model.GetHashCode()}+{path}";
		return _identifiers.TryGetValue(key, out var existing) ? existing : Add(key, new(model, path));
	}

	FieldIdentifier Add(string key, FieldIdentifier identifier)
	{
		_identifiers.Add(key, identifier);
		return identifier;
	}

	/*public bool HasErrors()
	{
		foreach (var identifier in _identifiers.Values)
		{
			if (_context.GetValidationMessages(identifier).AsValueEnumerable().Any())
			{
				return true;
			}
		}

		return false;
	}*/

	public void Clear()
	{
		_identifiers.Clear();
	}
}