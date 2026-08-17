using DragonSpark.Server.Requests;
using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Azure.Storage.Uploads;

public interface IUploadRoot : IFormatter<ClaimsPrincipal>, IFormatter<Input>;