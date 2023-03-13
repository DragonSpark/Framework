using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class ProblemSavingAutoSaveState : LogWarningException
{
	public ProblemSavingAutoSaveState(ILogger<ProblemSavingAutoSaveState> logger)
		: base(logger, "There was a problem saving auto save state") {}
}