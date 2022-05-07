using System.Net;
using System.Net.Http.Headers;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed record ResourceQueryRecord(string Address, HttpStatusCode Code, MediaTypeHeaderValue? ContentType);