using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class ResetRenderState : ValidatedCommand, IResetRenderState
{
	public ResetRenderState(IClearRenderState command) : base(new First(), command) {}
}