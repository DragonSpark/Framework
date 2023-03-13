using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class ProblemLoadingAutoSaveState : LogWarningException<string>
{
	public ProblemLoadingAutoSaveState(ILogger<ProblemLoadingAutoSaveState> logger)
		: base(logger, "There was a problem loading auto save state {Content}") {}
}