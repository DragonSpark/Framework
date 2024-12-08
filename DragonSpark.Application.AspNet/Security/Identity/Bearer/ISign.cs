using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public interface ISign : IFormatter<ClaimsIdentity>;