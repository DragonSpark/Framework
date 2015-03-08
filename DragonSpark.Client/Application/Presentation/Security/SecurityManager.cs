using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Security
{
	[Singleton( typeof(ISecurityManager), Priority = Priority.Lowest )]
	public class SecurityManager : ISecurityManager
	{
		readonly IDictionary<string, IEnumerable<ISecurityRule>> cache = new Dictionary<string, IEnumerable<ISecurityRule>>();

		public void Register( string name, IEnumerable<ISecurityRule> rules )
		{
			cache[ name ] = rules;
		}

		public bool Verify( IPrincipal principal, string rulesetName )
		{
			var rules = cache.ContainsKey( rulesetName ) ? cache[ rulesetName ] : Enumerable.Empty<ISecurityRule>();
			var result = rules.All( x => x.Check( principal ) );
			return result;
		}
	}
}