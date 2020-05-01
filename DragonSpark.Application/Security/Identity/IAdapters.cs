using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	public interface IAdapters : IAlteration<Task<AuthenticationState>> {}
}