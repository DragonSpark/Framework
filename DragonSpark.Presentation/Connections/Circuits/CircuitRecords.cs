﻿using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CircuitRecords : ReferenceValueTable<Circuit, CircuitRecord>
{
	public static CircuitRecords Default { get; } = new();

	CircuitRecords() {}
}