using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

public readonly record struct FormFileInput(IHeaderDictionary Headers, IFormFile Subject);