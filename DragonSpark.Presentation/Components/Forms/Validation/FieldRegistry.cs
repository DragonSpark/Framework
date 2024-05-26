using Microsoft.AspNetCore.Components.Forms;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class FieldRegistry
{
	readonly EditContext                         _context;
	readonly Dictionary<string, FieldIdentifier> _identifiers;

	public FieldRegistry(EditContext context) : this(context, new Dictionary<string, FieldIdentifier>()) {}

	public FieldRegistry(EditContext context, Dictionary<string, FieldIdentifier> identifiers)
	{
		_context     = context;
		_identifiers = identifiers;
	}

	public void Clear(Expression<Func<object?>> identifier)
	{
		_identifiers.Remove(identifier.ToString());
	}

	public FieldIdentifier Register<T>(Expression<Func<T>> identifier)
	{
		var key = identifier.ToString();
		return _identifiers.TryGetValue(key, out var existing) ? existing : Add(key, identifier);
	}

	FieldIdentifier Add<T>(string key, Expression<Func<T>> identifier)
	{
		var result = FieldIdentifier.Create(identifier);
		_identifiers.Add(key, result);
		return result;
	}

	public bool HasErrors()
	{
		foreach (var identifier in _identifiers.Values)
		{
			if (_context.GetValidationMessages(identifier).AsValueEnumerable().Any())
			{
				return true;
			}
		}

		return false;
	}

	public void Clear()
	{
		_identifiers.Clear();
	}
}