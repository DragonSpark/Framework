﻿using System;

namespace DragonSpark.Application.Hosting.xUnit;

sealed class EpochSpecimen : Specimen<DateTimeOffset>
{
	public static EpochSpecimen Default { get; } = new();

	EpochSpecimen() : base(Epoch.Default.Get) {}
}