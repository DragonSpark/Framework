﻿using System;

namespace DragonSpark.Diagnostics;

public sealed class EmptyBuilder : Builder
{
	public static EmptyBuilder Default { get; } = new();

	EmptyBuilder() : base(Polly.Policy.Handle<Exception>()) {}
}