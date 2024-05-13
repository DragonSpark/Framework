using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class ProblemLoadingState : LogWarningException<string>
{
	public ProblemLoadingState(ILogger<ProblemLoadingState> logger)
		: base(logger, "There was a problem loading auto save state {Content}") {}
}