using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

public readonly record struct FileSession(Guid? Workspace, Guid Session, IFormFile File);