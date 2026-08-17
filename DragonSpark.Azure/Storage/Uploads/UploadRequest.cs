namespace DragonSpark.Azure.Storage.Uploads;

public readonly record struct UploadRequest(Guid? Workspace, Guid Session, bool Last);