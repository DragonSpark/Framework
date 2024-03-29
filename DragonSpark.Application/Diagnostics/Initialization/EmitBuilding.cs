﻿namespace DragonSpark.Application.Diagnostics.Initialization;

sealed class EmitBuilding<T> : EmitProgramLog
{
	public static EmitBuilding<T> Default { get; } = new();

	EmitBuilding() : base(LogBuildingMessage<T>.Default) {}
}