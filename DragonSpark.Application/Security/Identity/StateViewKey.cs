﻿using DragonSpark.Application.Compose.Store;

namespace DragonSpark.Application.Security.Identity;

sealed class StateViewKey : Key<uint>
{
	public static StateViewKey Default { get; } = new();

	StateViewKey() : base(nameof(StateViewKey), x => x.ToString()) {}
}