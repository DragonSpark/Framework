using DragonSpark.Model.Results;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public class UploadRootAddress : Instance<Uri>
{
	protected UploadRootAddress(UploadSettings settings) : base(new(settings.Location)) {}
}