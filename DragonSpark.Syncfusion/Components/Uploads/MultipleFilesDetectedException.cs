using DragonSpark.Presentation.Components.Diagnostics;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public sealed class MultipleFilesDetectedException : ClientException
{
	public static MultipleFilesDetectedException Default { get; } = new();

	MultipleFilesDetectedException() : base("More than one file was detected.  Please only select one file.") {}
}