using System.Collections.Generic;
using System.Security.Principal;

namespace DragonSpark.Application.Presentation.Security
{
	public interface ISecurityManager
	{
		void Register( string name, IEnumerable<ISecurityRule> rules );

		bool Verify( IPrincipal principal, string rulesetName );
	}
}