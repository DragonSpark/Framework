using DragonSpark.Compose;
using DragonSpark.Server.Requests;
using System.Security.Claims;

namespace DragonSpark.Azure.Storage.Uploads;

sealed class UploadRoot : IUploadRoot
{
	readonly TemporaryUserPath _path;
	readonly string            _identifier;

	public UploadRoot(TemporaryUserPath path) : this(path, FileSessionIdentifier.Default) {}

	public UploadRoot(TemporaryUserPath path, string identifier)
	{
		_path       = path;
		_identifier = identifier;
	}

	public string Get(ClaimsPrincipal parameter)
		=> _path.Get(new(uint.Parse(parameter.FindFirstValue(ClaimTypes.NameIdentifier).Verify()),
		                 parameter.FindFirstValue(_identifier).Verify()));

	public string Get(Input parameter)
	{
		var (user, id) = parameter;
		var result = _path.Get(new(uint.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier).Verify()), id.ToString()));
		return result;
	}
}