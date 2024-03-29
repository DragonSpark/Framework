﻿using DragonSpark.Application.Compose;

namespace DragonSpark.Application.Hosting.Console;

sealed class ConsoleApplicationProfile : ApplicationProfile
{
	public static ConsoleApplicationProfile Default { get; } = new();

	ConsoleApplicationProfile() : base(_ => {}, _ => {}) {}
}