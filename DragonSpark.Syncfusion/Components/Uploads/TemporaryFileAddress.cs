using DragonSpark.Compose;
using Flurl;

namespace DragonSpark.SyncfusionRendering.Components.Uploads;

public class TemporaryFileAddress : IUploadFileAddress
{
	readonly Uri _address;

	protected TemporaryFileAddress(Uri address) => _address = address;

	public string Get(string parameter)
	{
		var directory = new FileInfo(parameter).Directory.Verify();
		return _address.AppendPathSegment(directory.Parent.Verify().Name)
		               .AppendPathSegment(directory.Name)
		               .AppendPathSegment(Path.GetFileName(parameter));
	}
}