using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class ProblemSavingState : LogWarningException
{
	public ProblemSavingState(ILogger<ProblemSavingState> logger)
		: base(logger, "There was a problem saving model state") {}
}

// TODO

public sealed class SavedContentMessage : LogMessage<string>
{
	public SavedContentMessage(ILogger<SavedContentMessage> logger) : base(logger, "Saved model content at {Size}") {}
}