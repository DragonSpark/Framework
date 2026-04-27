using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ComposeTypeKeys : ITypeKeys
{
	readonly DbContext            _context;
	readonly IGeneric<IKnownKeys> _keys;

	public ComposeTypeKeys(DbContext context)
		: this(context, Start.A.Generic(typeof(KnownKeys<>)).Of.Type<IKnownKeys>()) {}

	public ComposeTypeKeys(DbContext context, IGeneric<IKnownKeys> keys)
	{
		_context = context;
		_keys    = keys;
	}

	public ImmutableHashSet<object> Get(Type parameter) => _keys.Get(new[] { parameter })().Get(_context);
}