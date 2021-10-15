namespace DragonSpark.Application.Diagnostics.Initialization;

sealed class EmitBuilt<T> : EmitProgramLog
{
	public static EmitBuilt<T> Default { get; } = new();

	EmitBuilt() : base(LogBuiltMessage<T>.Default) {}
}