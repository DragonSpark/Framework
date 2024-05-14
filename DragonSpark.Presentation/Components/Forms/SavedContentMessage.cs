using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class SavedContentMessage : LogMessage<string>
{
	public SavedContentMessage(ILogger<SavedContentMessage> logger) : base(logger, "Saved model content at {Size}") {}
}