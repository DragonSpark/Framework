using System.Security.Claims;
using DragonSpark.Text;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public interface IMessageBearer : IFormatter<ClaimsIdentity>;