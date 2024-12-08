using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public interface IBearer : IFormatter<ClaimsIdentity>;