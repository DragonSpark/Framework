using System.Security.Principal;

namespace DragonSpark.Application.Presentation.Security
{
	public interface ISecurityRule
	{
		bool Check( IPrincipal principal );
	}
}