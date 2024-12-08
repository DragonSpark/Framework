﻿using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

sealed class SelectSetting : StartWhere<string, Setting>
{
	public static SelectSetting Default { get; } = new();

	SelectSetting() : base((p, x) => x.Id == p) {}
}