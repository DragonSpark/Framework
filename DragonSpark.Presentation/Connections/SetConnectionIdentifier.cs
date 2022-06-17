﻿using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Connections;

sealed class SetConnectionIdentifier : IResult<Guid>
{
	readonly SessionIdentifier           _identifier;
	readonly PersistConnectionIdentifier _persist;

	public SetConnectionIdentifier(SessionIdentifier identifier, PersistConnectionIdentifier persist)
	{
		_identifier = identifier;
		_persist    = persist;
	}

	public Guid Get()
	{
		var result = _identifier.Get();
		_persist.Execute(result);
		return result;
	}
}