﻿namespace DragonSpark.Runtime.Environment;

sealed class DevelopmentRuntimeFile : RuntimeFile
{
	public static DevelopmentRuntimeFile Default { get; } = new();

	DevelopmentRuntimeFile() : base(".runtimeconfig.dev.json") {}
}