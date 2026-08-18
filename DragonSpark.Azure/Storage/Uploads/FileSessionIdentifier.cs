using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class FileSessionIdentifier : Claim
{
	public static FileSessionIdentifier Default { get; } = new();

	FileSessionIdentifier() : base("file-upload-identifier") {}
}